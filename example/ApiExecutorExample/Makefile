.PHONY: help start-openfga stop-openfga run run-all clean

help:
	@echo "ApiExecutor Example - Makefile Commands"
	@echo ""
	@echo "  make start-openfga  - Start OpenFGA server in Docker on port 8080"
	@echo "  make stop-openfga   - Stop the OpenFGA Docker container"
	@echo "  make run            - Run the ApiExecutor example (requires OpenFGA running)"
	@echo "  make run-all        - Start OpenFGA, run example, then stop OpenFGA"
	@echo "  make clean          - Clean build artifacts"
	@echo ""

# Start OpenFGA server in Docker
start-openfga:
	@echo "Starting OpenFGA server on localhost:8080..."
	@docker run -d --name openfga-example -p 8080:8080 openfga/openfga:latest run
	@echo "Waiting for OpenFGA to be ready..."
	@sleep 3
	@curl -s http://localhost:8080/healthz || (echo "OpenFGA failed to start" && exit 1)
	@echo "✅ OpenFGA is ready!"

# Stop OpenFGA server
stop-openfga:
	@echo "Stopping OpenFGA server..."
	@docker stop openfga-example 2>/dev/null || true
	@docker rm openfga-example 2>/dev/null || true
	@echo "✅ OpenFGA stopped"

# Run the example
run:
	@echo "Running ApiExecutor example..."
	@dotnet run --project ApiExecutorExample.csproj

# Run everything: start server, run example, stop server
run-all: start-openfga
	@echo ""
	@$(MAKE) run
	@echo ""
	@$(MAKE) stop-openfga

# Clean build artifacts
clean:
	@echo "Cleaning build artifacts..."
	@dotnet clean
	@rm -rf bin obj
	@echo "✅ Clean complete"
