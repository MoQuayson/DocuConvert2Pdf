#  Word to PDF Converter Background Service (LibreOffice 7) for .NET 6

## Overview
This .NET 6 Background Service Worker is designed to convert bulk Word documents to PDFs using LibreOffice 7. The service runs in the background, monitoring a specified directory for incoming Word documents and automatically converting them to PDF format.

## Prerequisites
- .NET 6 SDK: Ensure you have the .NET 6 SDK installed on your machine. You can download it [here](https://dotnet.microsoft.com/download/dotnet/6.0).

- LibreOffice 7: Install LibreOffice 7 on your system. You can download it [here](https://www.libreoffice.org/download/download/).

## Configuration
### 1. Configure LibreOffice Path:
- Open the appsettings.json file.
- Update the LibreOfficePath setting with the path to your LibreOffice 7 installation.
- If soffice is globally enabled, set IsGlobal to **true**

```json
"LibreOfficeConfig": {
    "IsGlobal": false,
    "Path": "C:\\Program Files\\LibreOffice\\program\\soffice.exe"
  },
```

### 2. Configure Where Word and PDF Documents Storage Path:
- Open the appsettings.json file.
- Update the DocFilesPath setting with the path to the directory where Word documents will be placed for conversion.
- Update the DocFilesPath setting with the path to the directory where PDF documents will be stored.

```json
"DocumentConversionConfig": {
    "DocFilesPath": "C:\\DocConv\\docx",
    "PdfFilesPath": "C:\\DocConv\\pdf"
  }
```

### Restore Packages
```bash
  dotnet restore
```

### Build and Run the Application
```bash
  dotnet build
  dotnet run
```

## Authors

- [@MoQuayson](https://www.github.com/MoQuayson)

