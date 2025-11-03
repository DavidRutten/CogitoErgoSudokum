.PHONY: build run clean publish-osx publish-win restore check-sdk

# Check if .NET SDK is installed
check-sdk:
	@dotnet --version > /dev/null 2>&1 || (echo "❌ .NET SDK not found. Install from: https://dotnet.microsoft.com/download" && exit 1)
	@echo "✅ .NET SDK found: $$(dotnet --version)"

# Restore dependencies
restore: check-sdk
	dotnet restore

# Build the project
build: restore
	dotnet build -c Release

# Run the project
run: restore
	dotnet run

# Clean build artifacts
clean:
	dotnet clean
	rm -rf bin obj

# Publish for macOS (ARM64 - M1/M2/M3)
publish-osx-arm: build
	dotnet publish -c Release -r osx-arm64 --self-contained -o ./publish/osx-arm64
	@echo "✅ Published to ./publish/osx-arm64/"

# Publish for macOS (Intel)
publish-osx-intel: build
	dotnet publish -c Release -r osx-x64 --self-contained -o ./publish/osx-x64
	@echo "✅ Published to ./publish/osx-x64/"

# Publish for Windows
publish-win: build
	dotnet publish -c Release -r win-x64 --self-contained -o ./publish/win-x64
	@echo "✅ Published to ./publish/win-x64/"

# Publish for all platforms
publish-all: publish-osx-arm publish-osx-intel publish-win
	@echo "✅ Published for all platforms"
