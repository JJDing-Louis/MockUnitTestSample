# MockUnitTestSample

這是一個給單元測試初學者使用的 .NET 10 範例。它示範如何用 Moq 代替資料庫與通知服務，再用 Bogus 產生不含真實個資的測試資料。

範例情境是「找出即將到期的工作項目，並逐筆寄送提醒」。專案只做單元測試，不會真的連線到資料庫，也不會寄出 Email。

## 專案結構

```text
MockUnitTestSample/
├── src/MockUnitTestSample/
│   ├── INotificationService.cs
│   ├── IWorkItemRepository.cs
│   ├── WorkItem.cs
│   ├── WorkItemReminderService.cs
│   └── WorkItemStatus.cs
└── tests/MockUnitTestSample.Tests/
    ├── WorkItemFakerFactory.cs
    └── WorkItemReminderServiceTests.cs
```

## 執行方式

電腦需要先安裝 .NET 10 SDK。

```bash
git clone https://github.com/JJDing-Louis/MockUnitTestSample.git
cd MockUnitTestSample
dotnet test MockUnitTestSample.slnx --configuration Release
```

測試涵蓋有兩筆資料、沒有資料，以及提醒天數超出允許範圍的情境。

## 使用套件

- NUnit：執行測試。
- Fluent Assertions：用接近自然語句的方式核對結果。
- Moq：建立 Repository 與通知服務的替身。
- Bogus：按照規則產生測試資料。

本專案使用 MIT License。Fluent Assertions 7.2.2 採 Apache 2.0 License；若改用 8.x 以上版本，請先確認使用情境是否符合新版授權條款。
