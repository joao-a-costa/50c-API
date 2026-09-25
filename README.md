# Sage50c-API (Forked)

![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4) ![Sage 50c](https://img.shields.io/badge/Sage%2050c-2022-00D639) ![License](https://img.shields.io/badge/license-MIT-blue)

Samples, templates and extensibility projects for building on top of the **Sage 50c** API.

---

## 🎯 Purpose

This repository is a fork of the [Sage Portugal 50c-API](https://github.com/sage-portugal/50c-API). The original repository stopped responding to issues and pull requests, so this fork is actively maintained: it fixes outstanding problems and adds new samples and features based on real integration work.

## ✨ Features

- 🧩 **API sample app** (`Sage50c.API.Sample`) covering items, customers, suppliers, stock, item and account transactions, printing and PDF export.
- ⚙️ **System settings / data manager viewers** that dump `SystemSettings` and `DataManager.Settings` as JSON.
- 🔌 **Extensibility samples** for adding tabs and behaviour inside Sage 50c.
- 🧪 **COM serialization** helpers for turning Sage COM objects into JSON.
- 🛠️ **Templates** for starting a new API or extensibility project.

## 🏗️ Architecture

| Solution | Purpose |
|----------|---------|
| `Sage50c.API.Sample/Sage50c.API.Sample.sln` | Main WinForms sample for the Sage 50c API |
| `Sage50c.API.Template/Sage50c.API.Template.sln` | Starting template for new API projects |
| `Sage50c.API.COM.Serialization/Sage50c.API.COM.Serialization.sln` | JSON serialization of Sage COM objects |
| `Sage50c.Extensibility.Sample/Sage50c.Extensibility.Sample.sln` | Extensibility (add-in) sample |
| `Sage50c.Extensibility.CustomerTab/Sage50c.Extensibility.CustomerTab.sln` | Extra tab on the customer form |
| `Sage50c.API.PromotionPrice/Sage50c.API.PromotionPrice.sln` | Promotion price sample (.NET Framework) |
| `Sage50c.API.NetCore.PromotionPrice/Sage50c.API.NetCore.PromotionPrice.sln` | Promotion price sample (.NET Core) |
| `Sage50c.API.NetCore.UserValidation/Sage50c.API.NetCore.UserValidation.sln` | User validation sample (.NET Core) |
| `Sage50c.API.ReturnablePackages/Sage50c.API.ReturnablePackages.sln` | Returnable packages sample |
| `Sage50c.API.VendaComDoisCamposExtra/Sage50c.API.VendaComDoisCamposExtra.sln` | Sale with two extra fields sample |

Shared code lives in `Sage50c.Common`; Sage interop assemblies are under `Interops` and `dependencies`.

## 🛠️ Tech Stack

- **.NET Framework 4.8** (x86) for the WinForms samples, **.NET Core** for the `NetCore` samples
- **Sage 50c 2022** COM interops (`S50cSys22`, `S50cDL22`, `S50cBO22`, `S50cPrint22`, …)
- **Newtonsoft.Json 13** with Sage's `JsonCOMSerialization` converters (`APIEngine.Serializer`)

## 🚀 Quick Start

### Prerequisites

- **Sage 50c** installed (interops are referenced from `C:\Program Files (x86)\Common Files\sage\2070\50c2022\Interops`)
- **Visual Studio 2019** or later

### Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/joao-a-costa/50c-API.git
   ```
2. Open `Sage50c.API.Sample/Sage50c.API.Sample.sln` in Visual Studio.
3. Restore NuGet packages.
4. Build for **x86** — the Sage interops are 32-bit.

## 📋 Configuration

Start the sample, sign in to a Sage 50c company, and use the tabs to exercise each area of the API. The **System Settings** tab serialises `APIEngine.SystemSettings` to JSON; the **Data Manager** tab does the same for `APIEngine.DataManager.Settings`.

## 🩺 Troubleshooting

### `ITypeInfo::GetTypeAttr devolveu hr=80004001` when serialising settings

Some child COM objects exposed by Sage (seen with `SystemSettings.FiscalTrace`, typed as the empty `IFiscalTraceLog` interface) return type info that doesn't implement `GetTypeAttr` (E_NOTIMPL) on some machines. Serialising the whole object in one call then fails.

`SystemSettingsUC` serialises each property on its own. Properties are read from the declared interop type (a COM object is `System.__ComObject` at runtime). A property that fails shows `<ExceptionType: message>` in the output, and the rest still serialise. If it only fails on one machine, compare the Sage 50c version/updates with a machine where it works.

## 📚 Documentation

- Detailed documentation is in the `doc` directory.
- The original Sage 50c documentation covers general usage.
- Source comments explain specific functions.

## 🤝 Contributing

- Report bugs and suggest features via [Issues](https://github.com/joao-a-costa/50c-API/issues).
- Submit pull requests with fixes or new samples.

## 📝 License

MIT — see [LICENSE](LICENSE). Thanks to the original authors of the [Sage Portugal 50c-API](https://github.com/sage-portugal/50c-API) and all contributors. Questions and feedback: [Discussions](https://github.com/joao-a-costa/50c-API/discussions).
