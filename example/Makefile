all: build

project_name=Example1
openfga_version=latest

restore:
	dotnet restore "${project_name}/${project_name}.csproj"

build: restore
	dotnet build "${project_name}/${project_name}.csproj" -c Release -o "${project_name}/bin"

run:
	sh -c "${project_name}/bin/${project_name}"

run-openfga:
	docker pull docker.io/openfga/openfga:${openfga_version} && \
		docker run -p 8080:8080 docker.io/openfga/openfga:${openfga_version} run
