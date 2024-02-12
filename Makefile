.PHONY: first-run
first-run:
	dotnet dev-certs https --trust
	test
	run

.PHONY: run
run:
	dotnet run --project PangeaApi/PangeaApi.csproj

.PHONY: test
test:
	dotnet test