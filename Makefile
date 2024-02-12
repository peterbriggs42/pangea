.PHONY: run
run:
	dotnet run --project PangeaApi/PangeaApi.csproj --launch-profile https

.PHONY: test
test:
	dotnet test

.PHONY: trust-certs
trust-certs:
	dotnet dev-certs https --trust

.PHONY: first-run
first-run: trust-certs test run