# 🚀 FTP Batch Uploader with Progress Bar

Ek intelligent Windows Forms application jo files ko efficiently FTP server par upload karti hai. Is project ko asynchronous programming (async/await) aur multi-threading ka use karke banaya gaya hai taaki performance fast rahe aur UI freeze na ho.

## ✨ Key Features

- **📁 Folder Selection:** User-friendly `FolderBrowserDialog` ke zariye kisi bhi folder ko select karne ki suvidha.
- **🔄 Batch Processing:** Files ko groups (batches) mein upload karne ka system taaki server par load na bade.
- **📊 Real-time Progress Bar:** Har file ke upload hone par progress bar accurately update hota hai.
- **📂 Auto-Directory Creation:** Agar FTP par folder structure nahi hai, toh application use automatically create kar deti hai.
- **🛡️ Intelligent Validation:** Khali folders (empty folders) ko pehle hi detect kar leta hai aur user ko "No files found" ka message dikhata hai.
- **✅ Move to Processed:** Successfully upload hone ke baad files automatically `Processed` folder mein move ho jati hain.

## 🛠️ Tech Stack

- **Language:** C#
- **Framework:** .NET (WinForms)
- **Settings Management:** Microsoft.Extensions.Configuration (JSON based)
- **Networking:** FtpWebRequest

## 🚀 Getting Started

### 1. Prerequisites
Aapke system mein **Visual Studio** aur **.NET SDK** installed hona chahiye.

### 2. Configuration (`appsettings.json`)
Program chalane se pehle `appsettings.json` file mein apni FTP details zaroor update karein:

```json
{
  "FtpSettings": {
    "PendingRoot": "D:\\YourSourceFolder",
    "ProcessedRoot": "D:\\YourProcessedFolder",
    "FtpUrl": "ftp://127.0.0.1/",
    "FtpUser": "your_username",
    "FtpPass": "your_password",
    "BatchSize": 5,
    "MaxParallelUploads": 3
  }
}