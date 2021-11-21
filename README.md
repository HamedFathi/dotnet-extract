![extract_icon](https://user-images.githubusercontent.com/8418700/141079117-f3bfea84-3d9d-48fb-9303-7208e6f6cc8b.png)

## [Nuget](https://www.nuget.org/packages/dotnet-extract)

[![Open Source Love](https://badges.frapsoft.com/os/mit/mit.svg?v=102)](https://opensource.org/licenses/MIT)
![Nuget](https://img.shields.io/nuget/v/dotnet-extract)
![Nuget](https://img.shields.io/nuget/dt/dotnet-extract)

```bash
// install
dotnet tool install --global dotnet-extract

// update
dotnet tool update dotnet-extract --global

// uninstall
dotnet tool uninstall dotnet-extract --global
```

## Options

| Option           | Required   | Description |
|------------------|:----------:|-------------|
|-s\|--source     |Yes         |The assemblies folder source path.|
|-d\|--destination|No         |The destination folder path. If you do not specify a path, the files are extracted in the `current directory`.|
|-p\|--pattern    |No          |Pattern of the names of the resources you want to extract so you should write `regex` on full resource name like `RazorClassLibrary.Areas.Library.Pages.Shared._Message.cshtml`|
|-r\|--replace    |No          |Strategy how to replace files that already exist at the destination. default is `replace all`. Other available options are **ask\|a** and **skip\|s**.|
|-l\|--list     |No         |Get list of embedded resources. You can you regex pattern with this too.|
|-e\|--extension|No|You can define your custom undetectable file extensions. The value is comma separated like `.txt.dat,.mkv,.cshtml.cs`|

## Usage

```bash
dotnet extract -s SOURCE_FOLDER -d DESTINATION_FOLDER -p .*Areas.* -r ask
```

<hr/>

<div>Icon from <a href="https://www.iconfinder.com/" title="Iconfinder">https://www.iconfinder.com</a> under Creative Commons Attribution-Share Alike 3.0 Unported License.</div>
