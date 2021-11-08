![folder](https://user-images.githubusercontent.com/8418700/140723829-35e676cb-92bd-4619-a9da-caedd3bfe809.png)


You can find the Nuget package [here](https://www.nuget.org/packages/dotnet-extract).

## Recipes

```bash
// install
dotnet tool install --global dotnet-extract

// update
dotnet tool update dotnet-extract --global

// uninstall
dotnet tool uninstall dotnet-extract --global
```

## Arguments

| Option           | Required   | Description |
|------------------|:----------:|-------------|
|-s\|--source     |Yes         |The assemblies folder source path.|
|-d\|--destination|No         |The destination folder path. If you do not specify a path, the files are extracted in the `current directory`.|
|-p\|--pattern    |No          |Pattern of the names of the resources you want to extract so you should write `regex` on full resource name like `RazorClassLibrary.Areas.Library.Pages.Shared._Message.cshtml`|
|-r\|--replace    |No          |Strategy how to replace files that already exist at the destination. default is `replace all`. Other available options are **ask\|a** and **skip\|s**.|
|-l\|--list     |No         |Get list of embedded resources. You can you regex pattern with this too.|
|-e\|--extension|No|You can define your custom undetectable file extensions. The value is comma separated like `.txt.dat,.mkv,.cshtml.cs`|

## Example

```bash
dotnet extract -s SOURCE_FOLDER -d DESTINATION_FOLDER -p .*Areas.* -r ask
```

<hr/>

<div>Icons made by <a href="https://www.freepik.com" title="Freepik">Freepik</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a></div>
