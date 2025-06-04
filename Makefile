ROOT_DIR:=$(shell dirname $(realpath $(firstword $(MAKEFILE_LIST))))

# Main test target - runs .NET 6.0 tests for CI/CD
.PHONY: test
test:
	dotnet test $(ROOT_DIR)/src/OpenFga.Sdk.Test/OpenFga.Sdk.Test.csproj

# NOTE: These framework-specific tests require the appropriate .NET environment:
# - test-framework: Requires Windows or Mono with full xUnit support
# - test-netcore31: Requires .NET Core 3.1 SDK

# Test .NET Framework 4.8 compatibility using Mono
.PHONY: test-framework
test-framework:
	@if command -v mono >/dev/null 2>&1; then \
		cd $(ROOT_DIR)/src/OpenFga.Sdk.Test.Framework && \
		dotnet build -c Debug && \
		mono /xunit/xunit.runner.console.2.1.0/tools/xunit.console.exe \
			$(ROOT_DIR)/src/OpenFga.Sdk.Test.Framework/bin/Debug/net48/OpenFga.Sdk.Test.Framework.dll; \
	else \
		echo "Mono is not installed. Skipping .NET Framework tests."; \
		exit 0; \
	fi

# Test .NET Core 3.1 compatibility
.PHONY: test-netcore31
test-netcore31:
	dotnet test $(ROOT_DIR)/src/OpenFga.Sdk.Test.NetCore31/OpenFga.Sdk.Test.NetCore31.csproj

# Run all tests - Note: Will only fully succeed on a Windows environment with all .NET versions installed
.PHONY: test-all
test-all: test 
	@echo "To run framework-specific tests, use 'make test-framework' and 'make test-netcore31' on appropriate environments"

.PHONY: build
build:
	dotnet build $(ROOT_DIR)/src/OpenFga.Sdk/OpenFga.Sdk.csproj

.PHONY: pack
pack:
	dotnet pack $(ROOT_DIR)/src/OpenFga.Sdk/OpenFga.Sdk.csproj -c Release