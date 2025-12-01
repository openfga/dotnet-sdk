//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 1.x
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://openfga.dev/community
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//


using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OpenFga.Sdk.Test.ApiClient {
    /// <summary>
    /// Unit tests for RetryHandler class
    /// </summary>
    public class RetryHandlerTests {
        #region Test Helpers

        /// <summary>
        /// Creates a RetryParams instance with default test values
        /// </summary>
        private static RetryParams CreateDefaultRetryParams(int maxRetry = 3, int minWaitInMs = 100) {
            return new RetryParams {
                MaxRetry = maxRetry,
                MinWaitInMs = minWaitInMs
            };
        }

        /// <summary>
        /// Creates a RetryHandler instance with default test configuration
        /// </summary>
        private static RetryHandler CreateRetryHandler(int maxRetry = 3, int minWaitInMs = 100) {
            var retryParams = CreateDefaultRetryParams(maxRetry, minWaitInMs);
            return new RetryHandler(retryParams);
        }

        /// <summary>
        /// Creates a mock HttpResponseMessage with optional Retry-After header
        /// </summary>
        private static HttpResponseMessage CreateMockResponse(
            HttpStatusCode statusCode,
            string retryAfterValue = null) {
            var response = new HttpResponseMessage {
                StatusCode = statusCode
            };

            if (!string.IsNullOrEmpty(retryAfterValue)) {
                response.Headers.TryAddWithoutValidation("Retry-After", retryAfterValue);
            }

            return response;
        }

        /// <summary>
        /// Creates HttpHeaders with a Retry-After header for testing
        /// </summary>
        private static HttpHeaders CreateHeadersWithRetryAfter(string retryAfterValue) {
            var response = new HttpResponseMessage();
            response.Headers.TryAddWithoutValidation("Retry-After", retryAfterValue);
            return response.Headers;
        }

        #endregion

        #region Transient Error Detection (IsTransientError)

        [Fact]
        public void IsTransientError_HttpRequestException_ReturnsTrue() {
            var handler = CreateRetryHandler(maxRetry: 3);
            var exception = new HttpRequestException("Network error");
            var attemptCount = 0;

            var result = handler.IsTransientError(exception, attemptCount);

            Assert.True(result, "HttpRequestException should be retryable");
        }

        [Fact]
        public void IsTransientError_SocketException_ReturnsTrue() {
            var handler = CreateRetryHandler(maxRetry: 3);
            var exception = new SocketException();
            var attemptCount = 0;

            var result = handler.IsTransientError(exception, attemptCount);

            Assert.True(result, "SocketException should be retryable");
        }

        [Fact]
        public void IsTransientError_TimeoutException_ReturnsTrue() {
            var handler = CreateRetryHandler(maxRetry: 3);
            var exception = new TimeoutException("Request timeout");
            var attemptCount = 0;

            var result = handler.IsTransientError(exception, attemptCount);

            Assert.True(result, "TimeoutException should be retryable");
        }

        [Fact]
        public void IsTransientError_TaskCanceledException_Timeout_ReturnsTrue() {
            var handler = CreateRetryHandler(maxRetry: 3);
            // TaskCanceledException without a cancellation token set means timeout
            var exception = new TaskCanceledException("Request timed out");
            var attemptCount = 0;

            var result = handler.IsTransientError(exception, attemptCount);

            Assert.True(result, "TaskCanceledException (timeout) should be retryable");
        }

        [Fact]
        public async Task IsTransientError_TaskCanceledException_UserCancelled_ReturnsFalse() {
            var handler = CreateRetryHandler(maxRetry: 3);
            var attemptCount = 0;

            // Create TaskCanceledException with a cancelled token in a framework-compatible way
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            TaskCanceledException exception;
            try {
                await Task.Delay(1000, cts.Token);
                exception = new TaskCanceledException();
            }
            catch (TaskCanceledException tce) {
                exception = tce;
            }

            var result = handler.IsTransientError(exception, attemptCount);

            Assert.False(result, "TaskCanceledException (user-initiated) should NOT be retryable");
        }

        [Fact]
        public void IsTransientError_ArgumentException_ReturnsFalse() {
            var handler = CreateRetryHandler(maxRetry: 3);
            var exception = new ArgumentException("Invalid argument");
            var attemptCount = 0;

            var result = handler.IsTransientError(exception, attemptCount);

            Assert.False(result, "ArgumentException is not transient and should NOT be retryable");
        }

        [Fact]
        public void IsTransientError_MaxRetriesExceeded_ReturnsFalse() {
            var handler = CreateRetryHandler(maxRetry: 3);
            var exception = new HttpRequestException("Network error");
            var attemptCount = 3;

            var result = handler.IsTransientError(exception, attemptCount);

            Assert.False(result, "Should not retry when max retries reached");
        }

        #endregion

        #region Exponential Backoff Calculation

        [Fact]
        public void CalculateExponentialBackoff_Attempt0_ReturnsCorrectRange() {
            var handler = CreateRetryHandler(maxRetry: 5, minWaitInMs: 500);
            var attemptCount = 0;

            // Run multiple times to account for jitter randomization
            for (int i = 0; i < 100; i++) {
                var delay = handler.CalculateDelayFromHeaders(null, attemptCount);

                // Formula: [2^0 * 500ms, 2^1 * 500ms] = [500ms, 1000ms]
                Assert.True(delay.TotalMilliseconds >= 500,
                    $"Delay {delay.TotalMilliseconds}ms should be >= 500ms (attempt {attemptCount})");
                Assert.True(delay.TotalMilliseconds <= 1000,
                    $"Delay {delay.TotalMilliseconds}ms should be <= 1000ms (attempt {attemptCount})");
            }
        }

        [Fact]
        public void CalculateExponentialBackoff_Attempt1_ReturnsCorrectRange() {
            var handler = CreateRetryHandler(maxRetry: 5, minWaitInMs: 500);
            var attemptCount = 1;

            for (int i = 0; i < 100; i++) {
                var delay = handler.CalculateDelayFromHeaders(null, attemptCount);

                // Formula: [2^1 * 500ms, 2^2 * 500ms] = [1000ms, 2000ms]
                Assert.True(delay.TotalMilliseconds >= 1000,
                    $"Delay {delay.TotalMilliseconds}ms should be >= 1000ms (attempt {attemptCount})");
                Assert.True(delay.TotalMilliseconds <= 2000,
                    $"Delay {delay.TotalMilliseconds}ms should be <= 2000ms (attempt {attemptCount})");
            }
        }

        [Fact]
        public void CalculateExponentialBackoff_Attempt2_ReturnsCorrectRange() {
            var handler = CreateRetryHandler(maxRetry: 5, minWaitInMs: 500);
            var attemptCount = 2;

            for (int i = 0; i < 100; i++) {
                var delay = handler.CalculateDelayFromHeaders(null, attemptCount);

                // Formula: [2^2 * 500ms, 2^3 * 500ms] = [2000ms, 4000ms]
                Assert.True(delay.TotalMilliseconds >= 2000,
                    $"Delay {delay.TotalMilliseconds}ms should be >= 2000ms (attempt {attemptCount})");
                Assert.True(delay.TotalMilliseconds <= 4000,
                    $"Delay {delay.TotalMilliseconds}ms should be <= 4000ms (attempt {attemptCount})");
            }
        }

        [Fact]
        public void CalculateExponentialBackoff_Attempt7_CapsAt120Seconds() {
            var handler = CreateRetryHandler(maxRetry: 15, minWaitInMs: 500);
            var attemptCount = 7;

            for (int i = 0; i < 100; i++) {
                var delay = handler.CalculateDelayFromHeaders(null, attemptCount);

                // Formula would be: [2^7 * 500ms, 2^8 * 500ms] = [64000ms, 128000ms]
                // But upper bound is capped at MAX_RETRY_DELAY_MS (120000ms = 120 seconds)
                Assert.True(delay.TotalMilliseconds <= 120000,
                    $"Delay {delay.TotalMilliseconds}ms should be capped at 120000ms (attempt {attemptCount})");
                Assert.True(delay.TotalMilliseconds >= 64000,
                    $"Delay {delay.TotalMilliseconds}ms should be >= 64000ms (attempt {attemptCount})");
            }
        }

        [Fact]
        public void CalculateExponentialBackoff_WithJitter_VariesWithinRange() {
            var handler = CreateRetryHandler(maxRetry: 5, minWaitInMs: 500);
            var attemptCount = 1;
            var delays = new List<double>();

            // Collect 100 samples to verify jitter randomization is working
            for (int i = 0; i < 100; i++) {
                var delay = handler.CalculateDelayFromHeaders(null, attemptCount);
                delays.Add(delay.TotalMilliseconds);
            }

            // Verify we have significant variation (jitter is working)
            var distinctDelays = delays.Distinct().Count();
            Assert.True(distinctDelays > 50,
                $"Expected significant variation due to jitter, but only got {distinctDelays} distinct values out of 100 samples");

            // Verify all delays are within expected range [1000ms, 2000ms]
            Assert.All(delays, d => {
                Assert.True(d >= 1000, $"Delay {d}ms should be >= 1000ms");
                Assert.True(d <= 2000, $"Delay {d}ms should be <= 2000ms");
            });

            // Verify delays are reasonably distributed (not all clustered at one end)
            var minDelay = delays.Min();
            var maxDelay = delays.Max();
            var range = maxDelay - minDelay;
            Assert.True(range > 500,
                $"Expected delays to be distributed across range, but range was only {range}ms (min: {minDelay}ms, max: {maxDelay}ms)");
        }

        [Fact]
        public void CalculateExponentialBackoff_CustomMinWait_UsesConfigValue() {
            var handler100ms = CreateRetryHandler(maxRetry: 5, minWaitInMs: 100);
            var handler1000ms = CreateRetryHandler(maxRetry: 5, minWaitInMs: 1000);
            var attemptCount = 0;

            // Test MinWaitInMs = 100: [2^0 * 100ms, 2^1 * 100ms] = [100ms, 200ms]
            for (int i = 0; i < 50; i++) {
                var delay = handler100ms.CalculateDelayFromHeaders(null, attemptCount);
                Assert.True(delay.TotalMilliseconds >= 100,
                    $"Delay {delay.TotalMilliseconds}ms should be >= 100ms (MinWaitInMs=100)");
                Assert.True(delay.TotalMilliseconds <= 200,
                    $"Delay {delay.TotalMilliseconds}ms should be <= 200ms (MinWaitInMs=100)");
            }

            // Test MinWaitInMs = 1000: [2^0 * 1000ms, 2^1 * 1000ms] = [1000ms, 2000ms]
            for (int i = 0; i < 50; i++) {
                var delay = handler1000ms.CalculateDelayFromHeaders(null, attemptCount);
                Assert.True(delay.TotalMilliseconds >= 1000,
                    $"Delay {delay.TotalMilliseconds}ms should be >= 1000ms (MinWaitInMs=1000)");
                Assert.True(delay.TotalMilliseconds <= 2000,
                    $"Delay {delay.TotalMilliseconds}ms should be <= 2000ms (MinWaitInMs=1000)");
            }
        }

        #endregion

        #region Hard Limits & Validation

        [Fact]
        public void Constructor_MaxRetry20_CapsAt15() {
            var retryParams = CreateDefaultRetryParams(maxRetry: 20);
            var handler = new RetryHandler(retryParams);

            // MAX_RETRIES_HARD_LIMIT is 15, so even though we requested 20, it should cap at 15
            // Verify by checking that attempt 15 is NOT retryable
            var shouldRetryAt15 = handler.ShouldRetry((HttpStatusCode)429, attemptCount: 15);
            Assert.False(shouldRetryAt15, "Should not retry at attempt 15 (hard limit exceeded)");

            // Verify that attempt 14 IS retryable
            var shouldRetryAt14 = handler.ShouldRetry((HttpStatusCode)429, attemptCount: 14);
            Assert.True(shouldRetryAt14, "Should retry at attempt 14 (within hard limit)");
        }

        [Fact]
        public void Constructor_MaxRetryNegative_ThrowsArgumentOutOfRangeException() {
            var retryParams = CreateDefaultRetryParams(maxRetry: -1);

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new RetryHandler(retryParams));
            Assert.Contains("MaxRetry", exception.Message);
        }

        [Fact]
        public void ShouldRetry_Attempt15_ReturnsFalse() {
            var handler = CreateRetryHandler(maxRetry: 15);

            var result = handler.ShouldRetry((HttpStatusCode)429, attemptCount: 15);

            Assert.False(result, "Should not retry at attempt 15 (max retries reached)");
        }

        [Fact]
        public void ShouldRetry_Attempt14_ReturnsTrue() {
            var handler = CreateRetryHandler(maxRetry: 15);

            var result = handler.ShouldRetry((HttpStatusCode)429, attemptCount: 14);

            Assert.True(result, "Should retry at attempt 14 (within max retries)");
        }

        [Fact]
        public void ShouldRetry_501NotImplemented_ReturnsFalse() {
            var handler = CreateRetryHandler(maxRetry: 3);

            var result = handler.ShouldRetry(HttpStatusCode.NotImplemented, attemptCount: 0);

            Assert.False(result, "501 Not Implemented should NOT be retried per RFC spec");
        }

        [Fact]
        public void ParseRetryAfter_Exactly1Second_ReturnsValue() {
            var handler = CreateRetryHandler();
            var headers = CreateHeadersWithRetryAfter("1");

            var result = handler.ParseRetryAfterFromHeaders(headers);

            Assert.NotNull(result);
            Assert.Equal(1, result.Value);
        }

        [Fact]
        public void ParseRetryAfter_Exactly1800Seconds_ReturnsValue() {
            var handler = CreateRetryHandler();
            var headers = CreateHeadersWithRetryAfter("1800");

            var result = handler.ParseRetryAfterFromHeaders(headers);

            Assert.NotNull(result);
            Assert.Equal(1800, result.Value);
        }

        [Fact]
        public void ParseRetryAfter_1801Seconds_ReturnsNull() {
            var handler = CreateRetryHandler();
            var headers = CreateHeadersWithRetryAfter("1801");

            var result = handler.ParseRetryAfterFromHeaders(headers);

            Assert.Null(result);
        }

        [Fact]
        public void ParseRetryAfter_0Seconds_ReturnsNull() {
            var handler = CreateRetryHandler();
            var headers = CreateHeadersWithRetryAfter("0");

            var result = handler.ParseRetryAfterFromHeaders(headers);

            Assert.Null(result);
        }

        #endregion

        #region ShouldRetry Decision Logic

        [Fact]
        public void ShouldRetry_429TooManyRequests_ReturnsTrue() {
            var handler = CreateRetryHandler(maxRetry: 3);

            var result = handler.ShouldRetry((HttpStatusCode)429, attemptCount: 0);

            Assert.True(result, "429 Too Many Requests should be retryable");
        }

        [Fact]
        public void ShouldRetry_500InternalServerError_ReturnsTrue() {
            var handler = CreateRetryHandler(maxRetry: 3);

            var result = handler.ShouldRetry(HttpStatusCode.InternalServerError, attemptCount: 0);

            Assert.True(result, "500 Internal Server Error should be retryable");
        }

        [Fact]
        public void ShouldRetry_502BadGateway_ReturnsTrue() {
            var handler = CreateRetryHandler(maxRetry: 3);

            var result = handler.ShouldRetry(HttpStatusCode.BadGateway, attemptCount: 0);

            Assert.True(result, "502 Bad Gateway should be retryable");
        }

        [Fact]
        public void ShouldRetry_503ServiceUnavailable_ReturnsTrue() {
            var handler = CreateRetryHandler(maxRetry: 3);

            var result = handler.ShouldRetry(HttpStatusCode.ServiceUnavailable, attemptCount: 0);

            Assert.True(result, "503 Service Unavailable should be retryable");
        }

        [Fact]
        public void ShouldRetry_504GatewayTimeout_ReturnsTrue() {
            var handler = CreateRetryHandler(maxRetry: 3);

            var result = handler.ShouldRetry(HttpStatusCode.GatewayTimeout, attemptCount: 0);

            Assert.True(result, "504 Gateway Timeout should be retryable");
        }

        [Fact]
        public void ShouldRetry_400BadRequest_ReturnsFalse() {
            var handler = CreateRetryHandler(maxRetry: 3);

            var result = handler.ShouldRetry(HttpStatusCode.BadRequest, attemptCount: 0);

            Assert.False(result, "400 Bad Request should NOT be retryable");
        }

        [Fact]
        public void ShouldRetry_404NotFound_ReturnsFalse() {
            var handler = CreateRetryHandler(maxRetry: 3);

            var result = handler.ShouldRetry(HttpStatusCode.NotFound, attemptCount: 0);

            Assert.False(result, "404 Not Found should NOT be retryable");
        }

        [Fact]
        public void ShouldRetry_200OK_ReturnsFalse() {
            var handler = CreateRetryHandler(maxRetry: 3);

            var result = handler.ShouldRetry(HttpStatusCode.OK, attemptCount: 0);

            Assert.False(result, "200 OK (success) should NOT be retryable");
        }

        #endregion

        #region ParseRetryAfter Edge Cases

        [Fact]
        public void ParseRetryAfter_EmptyString_ReturnsNull() {
            var handler = CreateRetryHandler();
            var headers = CreateHeadersWithRetryAfter("");

            var result = handler.ParseRetryAfterFromHeaders(headers);

            Assert.Null(result);
        }

        [Fact]
        public void ParseRetryAfter_WhitespaceOnly_ReturnsNull() {
            var handler = CreateRetryHandler();
            var headers = CreateHeadersWithRetryAfter("   ");

            var result = handler.ParseRetryAfterFromHeaders(headers);

            Assert.Null(result);
        }

        [Fact]
        public void ParseRetryAfter_NegativeInteger_ReturnsNull() {
            var handler = CreateRetryHandler();
            var headers = CreateHeadersWithRetryAfter("-10");

            var result = handler.ParseRetryAfterFromHeaders(headers);

            Assert.Null(result);
        }

        [Fact]
        public void ParseRetryAfter_NullHeaders_ReturnsNull() {
            var handler = CreateRetryHandler();

            var result = handler.ParseRetryAfterFromHeaders(null);

            Assert.Null(result);
        }

        [Fact]
        public void ParseRetryAfter_MultipleHeaderValues_UsesFirst() {
            var handler = CreateRetryHandler();
            var response = new HttpResponseMessage();
            response.Headers.TryAddWithoutValidation("Retry-After", "5");
            response.Headers.TryAddWithoutValidation("Retry-After", "10");

            var result = handler.ParseRetryAfterFromHeaders(response.Headers);

            Assert.NotNull(result);
            Assert.Equal(5, result.Value);
        }

        #endregion

        #region CalculateDelayFromHeaders Integration

        [Fact]
        public void CalculateDelay_WithRetryAfter_UsesHeaderValue() {
            var handler = CreateRetryHandler(maxRetry: 3, minWaitInMs: 500);
            var headers = CreateHeadersWithRetryAfter("5");

            var delay = handler.CalculateDelayFromHeaders(headers, attemptCount: 0);

            Assert.Equal(5000, delay.TotalMilliseconds);
        }

        [Fact]
        public void CalculateDelay_WithInvalidRetryAfter_UsesExponentialBackoff() {
            var handler = CreateRetryHandler(maxRetry: 3, minWaitInMs: 500);
            var headers = CreateHeadersWithRetryAfter("invalid");
            var attemptCount = 1;

            var delay = handler.CalculateDelayFromHeaders(headers, attemptCount);

            // Should use exponential backoff for attempt 1: [2^1 * 500ms, 2^2 * 500ms] = [1000ms, 2000ms]
            Assert.True(delay.TotalMilliseconds >= 1000, $"Expected >= 1000ms, got {delay.TotalMilliseconds}ms");
            Assert.True(delay.TotalMilliseconds <= 2000, $"Expected <= 2000ms, got {delay.TotalMilliseconds}ms");
        }

        [Fact]
        public void CalculateDelay_WithoutRetryAfter_UsesExponentialBackoff() {
            var handler = CreateRetryHandler(maxRetry: 3, minWaitInMs: 500);
            var response = new HttpResponseMessage();
            var attemptCount = 0;

            var delay = handler.CalculateDelayFromHeaders(response.Headers, attemptCount);

            // Should use exponential backoff for attempt 0: [2^0 * 500ms, 2^1 * 500ms] = [500ms, 1000ms]
            Assert.True(delay.TotalMilliseconds >= 500, $"Expected >= 500ms, got {delay.TotalMilliseconds}ms");
            Assert.True(delay.TotalMilliseconds <= 1000, $"Expected <= 1000ms, got {delay.TotalMilliseconds}ms");
        }

        [Fact]
        public void CalculateDelay_NullHeaders_UsesExponentialBackoff() {
            var handler = CreateRetryHandler(maxRetry: 3, minWaitInMs: 500);
            var attemptCount = 2;

            var delay = handler.CalculateDelayFromHeaders(null, attemptCount);

            // Should use exponential backoff for attempt 2: [2^2 * 500ms, 2^3 * 500ms] = [2000ms, 4000ms]
            Assert.True(delay.TotalMilliseconds >= 2000, $"Expected >= 2000ms, got {delay.TotalMilliseconds}ms");
            Assert.True(delay.TotalMilliseconds <= 4000, $"Expected <= 4000ms, got {delay.TotalMilliseconds}ms");
        }

        #endregion

        #region ExtractRetryAfterInfo (Error Reporting)

        [Fact]
        public void ExtractRetryAfterInfo_ValidHeader_ReturnsBothValues() {
            var handler = CreateRetryHandler();
            var headers = CreateHeadersWithRetryAfter("10");

            var (parsedSeconds, rawValue) = handler.ExtractRetryAfterInfoFromHeaders(headers);

            Assert.NotNull(parsedSeconds);
            Assert.Equal(10, parsedSeconds.Value);
            Assert.Equal("10", rawValue);
        }

        [Fact]
        public void ExtractRetryAfterInfo_InvalidHeader_ReturnsNullAndRaw() {
            var handler = CreateRetryHandler();
            var headers = CreateHeadersWithRetryAfter("invalid-value");

            var (parsedSeconds, rawValue) = handler.ExtractRetryAfterInfoFromHeaders(headers);

            Assert.Null(parsedSeconds);
            Assert.Equal("invalid-value", rawValue);
        }

        [Fact]
        public void ExtractRetryAfterInfo_MissingHeader_ReturnsNullBoth() {
            var handler = CreateRetryHandler();
            var response = new HttpResponseMessage();

            var (parsedSeconds, rawValue) = handler.ExtractRetryAfterInfoFromHeaders(response.Headers);

            Assert.Null(parsedSeconds);
            Assert.Null(rawValue);
        }

        [Fact]
        public void ExtractRetryAfterInfo_NullResponse_ReturnsNullBoth() {
            var handler = CreateRetryHandler();

            var (parsedSeconds, rawValue) = handler.ExtractRetryAfterInfoFromHeaders(null);

            Assert.Null(parsedSeconds);
            Assert.Null(rawValue);
        }

        #endregion
    }
}