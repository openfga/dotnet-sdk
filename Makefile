.PHONY: help test test-net48 test-net8 test-net9 build restore lint fmt check

# Default target when running just 'make'
help:
	@echo "Available targets:"
	@echo "  test       - Run all tests"
	@echo "  test-net48 - Run tests for .NET Framework 4.8"
	@echo "  test-net8  - Run tests for .NET 8.0"
	@echo "  test-net9  - Run tests for .NET 9.0"
	@echo "  lint       - Verify code formatting and style"
	@echo "  fmt        - Apply code formatting"
	@echo "  check      - Run all checks (lint + test)"

# Run tests for all supported frameworks
test: test-net48 test-net8 test-net9
	@echo "✅ All tests completed successfully!"

# Restore NuGet packages
restore:
	@echo "📦 Restoring NuGet packages..."
	@dotnet restore ./OpenFga.Sdk.sln

# Build the solution
build: restore
	@echo "🔨 Building solution..."
	@dotnet build ./OpenFga.Sdk.sln --no-restore

# Run tests for .NET Framework 4.8
test-net48: build
	@echo "🚀 Running tests for .NET Framework 4.8..."
	@dotnet test --framework net48 --no-build

# Run tests for .NET 8.0
test-net8: build
	@echo "🚀 Running tests for .NET 8.0..."
	@dotnet test --framework net8.0 --no-build

# Run tests for .NET 9.0
test-net9: build
	@echo "🚀 Running tests for .NET 9.0..."
	@dotnet test --framework net9.0 --no-build
# Verify code formatting and analyzers
lint:
	@echo "🔍 Checking code formatting..."
	@dotnet format --verify-no-changes --severity info || (echo "❌ Code formatting issues found. Run 'make fmt' to fix." && exit 1)
	@echo "✅ Code formatting looks good!"

# Apply formatting fixes
fmt:
	@echo "🎨 Applying code formatting..."
	@# Create backups of project files
	@cp src/OpenFga.Sdk/OpenFga.Sdk.csproj src/OpenFga.Sdk/OpenFga.Sdk.csproj.bak
	@cp src/OpenFga.Sdk.Test/OpenFga.Sdk.Test.csproj src/OpenFga.Sdk.Test/OpenFga.Sdk.Test.csproj.bak
	
	@# Use sed with appropriate flags for platform compatibility
	@if [ "$$(uname)" = "Darwin" ]; then \
		sed -i '' "s/<TargetFrameworks>.*<\\/TargetFrameworks>/<TargetFramework>net8.0<\\/TargetFramework>/" src/OpenFga.Sdk/OpenFga.Sdk.csproj && \
		sed -i '' "s/<TargetFrameworks>.*<\\/TargetFrameworks>/<TargetFramework>net8.0<\\/TargetFramework>/" src/OpenFga.Sdk.Test/OpenFga.Sdk.Test.csproj; \
	else \
		sed -i "s/<TargetFrameworks>.*<\\/TargetFrameworks>/<TargetFramework>net8.0<\\/TargetFramework>/" src/OpenFga.Sdk/OpenFga.Sdk.csproj && \
		sed -i "s/<TargetFrameworks>.*<\\/TargetFrameworks>/<TargetFramework>net8.0<\\/TargetFramework>/" src/OpenFga.Sdk.Test/OpenFga.Sdk.Test.csproj; \
	fi
	
	@# Run formatting
	@dotnet restore ./OpenFga.Sdk.sln
	@dotnet format ./OpenFga.Sdk.sln
	
	@# Restore original project files
	@mv src/OpenFga.Sdk/OpenFga.Sdk.csproj.bak src/OpenFga.Sdk/OpenFga.Sdk.csproj
	@mv src/OpenFga.Sdk.Test/OpenFga.Sdk.Test.csproj.bak src/OpenFga.Sdk.Test/OpenFga.Sdk.Test.csproj
	@echo "✅ Code formatting applied successfully!"

# Convenience target to run all checks
check: lint test
	@echo "✨ All checks completed successfully!"