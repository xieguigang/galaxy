---
name: FeatureAPI-清除编译警告
overview: 清理 Microsoft.Windows.FeatureAPI 工程编译产生的 104 条警告（BC42030/BC42108、SYSLIB0051、SYSLIB0003、SYSLIB0021、BC42300），全部通过修改源码真实修复，不使用 SuppressMessage 特性或 NoWarn 抑制。
todos:
  - id: remove-serialization-ctors
    content: 删除 7 个异常类的 Serializable 特性与序列化构造函数，消除 SYSLIB0051×7
    status: completed
  - id: fix-core-warnings
    content: 修复 Core 目录下 SYSLIB0003×4、BC42108×1 与 BC42030×6
    status: completed
  - id: fix-shell-common-property
    content: 修复 Shell\Common 与 PropertySystem 下 BC42030×24，并将 hashProvider 改为 MD5.Create()
    status: completed
  - id: fix-shell-dialogs-folders
    content: 修复 Shell\CommonFileDialogs 与 KnownFolders 下 BC42030×26
    status: completed
  - id: fix-shell-watcher-taskbar
    content: 修复 Shell\ShellObjectWatcher、Taskbar、Interop 下 BC42030×23
    status: completed
  - id: fix-sensors-shellext
    content: 修复 Sensors 与 ShellExtensions 下 BC42030×11 与 BC42108×2
    status: completed
  - id: fix-designer-xml-comments
    content: 移动 4 个资源 Designer 文件的 XML 注释块，消除 BC42300×4
    status: completed
  - id: verify-all-configs
    content: 对 Debug/AnyCPU、Debug/x64、Release/AnyCPU 三配置重建验证警告归零
    status: completed
    dependencies:
      - remove-serialization-ctors
      - fix-core-warnings
      - fix-shell-common-property
      - fix-shell-dialogs-folders
      - fix-shell-watcher-taskbar
      - fix-sensors-shellext
      - fix-designer-xml-comments
---

## 用户需求

清除 `Microsoft.Windows.FeatureAPI.vbproj` 项目编译时产生的全部警告消息，要求**通过真实的代码修复来消除警告**，禁止使用 `SuppressMessage` 等抑制特性、`NoWarn` 项目属性或 `#Disable Warning` 编译指令来绕过。

## 产品概述

本项目是 Microsoft Windows API Code Pack 的 VB.NET 移植版（Windows Shell / 任务栏 / 传感器 / 电源管理 / 属性系统等 Windows 桌面特性封装库）。当前一次完整重建会产生 **110 条警告**，其中本工程自身 **104 条**，其余 6 条来自工作区之外的被引用工程 `Core.vbproj`。

## 核心范围

- **目标**：本工程 `g:\galaxy\src\Microsoft.Windows.FeatureAPI\` 下的 104 条警告清零
- **不在范围内**：被引用工程 `g:\GCModeller\...\Microsoft.VisualBasic.Core\src\Core.vbproj` 产生的 6 条警告（SYSLIB0006×1、CA1416×5）保持原样
- **约束**：不得新增任何抑制手段；保留现有 `<NoWarn>$(NoWarn);WFO1000</NoWarn>` 不动

## 警告分类与数量

| 编号 | 数量 | 说明 |
| --- | --- | --- |
| BC42030 | 85 | 变量在赋值前以 ByRef 方式传入 COM Interop / P/Invoke 调用 |
| SYSLIB0051 | 7 | 异常类中过时的 BinaryFormatter 序列化构造函数 |
| SYSLIB0003 | 4 | `PermissionSetAttribute` 代码访问安全性（CAS）特性已过时 |
| BC42300 | 4 | 资源 Designer 文件中 XML 文档注释位置错误 |
| BC42108 | 3 | 结构体版本的前述 ByRef 未初始化问题 |
| SYSLIB0021 | 1 | `MD5CryptoServiceProvider` 已过时 |


## 验收标准

对 `Debug|AnyCPU`、`Debug|x64`、`Release|AnyCPU` 三种配置各执行一次 `-t:Rebuild`，FeatureAPI 工程路径下警告数为 0，编译 0 错误，仅残留被引用工程 Core.vbproj 的 6 条警告。

## 技术栈

- 语言/框架：Visual Basic .NET，Windows Desktop SDK（`Microsoft.NET.Sdk.WindowsDesktop`）
- 目标框架：`net10.0-windows`，启用 `UseWindowsForms` + `UseWPF` + `GenerateDocumentationFile`
- 工具链：.NET SDK 10.0.400（已验证），MSBuild 文件日志器
- 依赖：项目引用 `..\..\..\GCModeller\src\runtime\sciBASIC#\Microsoft.VisualBasic.Core\src\Core.vbproj`

## 实施策略

全部采用**语义等价的真实修复**，不引入任何抑制机制。核心思路：把编译器的「确定性赋值分析」缺口补齐、删除 .NET 9/10 中已被运行时废弃的死代码、把过时的 BCL API 替换为官方推荐的替代 API。

### 各类警告的修复决策与权衡

**1. BC42030（85 条）/ BC42108（3 条）—— 声明处显式初始化**

成因：变量声明后直接作为 ByRef 实参传给 COM Interop 或 P/Invoke 调用，例如：

```
Dim nativeShellItem As IShellItem2
Dim retCode As Integer = ShellNativeMethods.SHCreateItemFromParsingName(parsingName, IntPtr.Zero, guid, nativeShellItem)
```

修复：声明处追加 `= Nothing`。

- 决策理由：VB 中引用类型局部变量的默认值本就是 `Nothing`，结构体会被零初始化，因此 `= Nothing` 是**零开销、语义完全等价**的写法（JIT 不会额外生成指令），既满足编译器的确定性赋值分析，又提升了代码可读性与防御性。这是此类警告的标准修复方式。
- 风险与兜底：对于 BC42108（结构体），若 `= Nothing` 未能消除警告，则改用 `= New <结构体类型>()` 显式调用无参构造兜底，执行时逐个重新编译验证。
- 覆盖 32 个源文件，其中 `CommonFileDialog.vb`（10 处）、`KnownFolderHelper.vb`（5 处）、`JumpList.vb`（6 处）、`Sensor.vb`（6 处）、`ShellLibrary.vb`（7 处）为高发文件。

**2. SYSLIB0051（7 条）—— 删除序列化构造函数（已与用户确认）**

修复：删除 7 个异常类中的 `<Serializable>` 特性、`Protected Sub New(info As SerializationInfo, context As StreamingContext)` 及其 XML 注释块。

- 决策理由：.NET 9/10 已彻底移除 BinaryFormatter 序列化支持，这些构造函数在运行时必然抛异常，属于确定性死代码。微软官方对 SYSLIB0051 的推荐处置即为删除。
- 安全性已验证：已全库检索确认这 7 个类在项目内**均无派生类**（无 `Inherits` 引用）、**无 `GetObjectData` 重写**，删除后无编译期副作用。代价仅为公开 API 面减少一个 `Protected` 构造函数。
- 涉及文件：`ApplicationRecoveryException.vb`、`PowerManagerException.vb`、`LinguisticException.vb`、`SensorPlatformException.vb`、`ShellException.vb`、`CommonControlException.vb`、`PropertySystemException.vb`。

**3. SYSLIB0003（4 条）—— 删除 CAS 特性**

修复：删除 `Core\PowerManagement\PowerManager.vb` 第 168、197 行的 `<System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.Demand, Name:="FullTrust")>`（每处产生 2 条警告：特性本身 + `SecurityAction` 枚举）。

- 决策理由：CAS 在 .NET Core/5+ 完全不被运行时支持，该特性是纯装饰性死代码，删除后行为完全不变。
- 可选清理：同文件第 160、189 行 XML 文档中的 `<permission cref="System.Security.Permissions.SecurityPermission">` 标签可一并移除（不影响警告，但保持文档一致性）。

**4. SYSLIB0021（1 条）—— 替换过时加密类型**

修复：`Shell\Common\ShellObject.vb` 第 409 行

```
Private Shared hashProvider As New MD5CryptoServiceProvider()   ' 改为：
Private Shared hashProvider As MD5 = MD5.Create()
```

- 决策理由：`MD5.Create()` 是 BCL 官方推荐的工厂写法，返回 `MD5`（`HashAlgorithm` 派生），`ComputeHash` 签名与调用点（第 400 行 `ShellObject.hashProvider.ComputeHash(pidlData)`）完全兼容，无需改动调用方。
- 该文件第 6 行已有 `Imports System.Security.Cryptography`，无需新增导入。
- 注意：`MD5` 实例为 `Shared` 字段，与原 `MD5CryptoServiceProvider` 一样存在跨线程复用问题，本次保持原有行为不做额外改造（避免超出范围的架构变更）。

**5. BC42300（4 条）—— 移动 XML 文档注释位置**

修复：将 4 个资源 Designer 文件第 14-16 行的 `''' <summary>...</summary>` 块整体下移，使其**紧邻** `Friend Class` 声明（即置于三个 `<...Attribute>` 之后）。

- 决策理由：XML 文档注释必须直接位于所修饰的语言元素之前，中间不能插入普通注释或特性声明。这是语法层面的正确性问题，移动位置即为真实修复，同时让自动生成的资源类重新获得文档说明。
- 涉及文件：`ExtendedLinguisticServicesLocalizedMessages.Designer.vb`、`SensorsLocalizedMessages.Designer.vb`、`ShellLocalizedMessages.Designer.vb`、`ShellExtensionsLocalizedMessages.Designer.vb`。

## 执行要点（防止回归）

- **编译命令必须显式指定 vbproj**：同目录下存在 `Microsoft.Windows.FeatureAPI.slnx`，直接 `dotnet build` 会报 MSB4126（解决方案平台名为 `Any CPU` 而非 `AnyCPU`）。正确命令：

```
dotnet build Microsoft.Windows.FeatureAPI.vbproj -c Debug -p:Platform=AnyCPU -t:Rebuild -v m -nologo
```

- **缩进风格必须保持原样**：`Shell\` 目录下文件多为 **Tab** 缩进，`Core\`、`Sensors\` 下多为**空格**缩进，编辑时不得混用，避免产生无意义的大 diff。
- **变更半径控制**：仅做上述定点修改，不重构无关逻辑、不调整项目属性（`NoWarn` 现有 WFO1000 保留）。
- **渐进验证**：每完成一个批次立即重建，比对警告数下降是否符合预期（预期每批归零对应编号），避免最后一次性排查。
- **MSB3270 说明**：首次增量构建时曾出现处理器架构不匹配警告（本工程 MSIL vs 引用工程 AMD64），干净重建后已消失，本次不处理；若复现，属被引用工程 `Core.vbproj` 中 `<PlatformTarget>x64</PlatformTarget>` 导致，不在本次范围内。

## 架构设计

本次修改为**同层定点修复**，不涉及架构调整。按「警告类型 × 目录」二维切分任务，保证每批次文件集合不重叠、上下文切换最小：

- 横向切分：先跨目录处理 7 个异常类的 SYSLIB0051（同构、一次性改完）
- 纵向切分：BC42030 按目录分批（Core → Shell/Common+PropertySystem → Shell/CommonFileDialogs+KnownFolders → Shell/ShellObjectWatcher+Taskbar+Interop → Sensors+ShellExtensions）
- 最后独立处理 4 个 Designer 文件与全配置回归验证

## 目录结构

```
g:\galaxy\src\Microsoft.Windows.FeatureAPI\
├── Core\
│   ├── AppRestartRecovery\ApplicationRecoveryException.vb   # [MODIFY] 删除 <Serializable> 与序列化构造函数（SYSLIB0051）
│   ├── PowerManagement\PowerManagerException.vb             # [MODIFY] 删除 <Serializable> 与序列化构造函数（SYSLIB0051）
│   ├── PowerManagement\PowerManager.vb                      # [MODIFY] 删除 2 处 PermissionSetAttribute（SYSLIB0003×4）
│   ├── PowerManagement\Power.vb:11                          # [MODIFY] powerCap 声明处加 = Nothing（BC42108）
│   ├── Dialogs\TaskDialogs\TaskDialog.vb                    # [MODIFY] 4 处变量初始化（BC42030×4：801/811/968/980）
│   └── PropertySystem\PropVariant.vb                        # [MODIFY] 2 处 action 变量初始化（BC42030×2：156/709）
├── ExtendedLinguisticServices\
│   ├── LinguisticException.vb                               # [MODIFY] 删除序列化构造函数（SYSLIB0051）
│   └── Resources\ExtendedLinguisticServicesLocalizedMessages.Designer.vb  # [MODIFY] 移动 XML 注释块（BC42300）
├── Sensors\
│   ├── ObjectModel\SensorPlatformException.vb               # [MODIFY] 删除序列化构造函数（SYSLIB0051）
│   ├── ObjectModel\Sensor.vb                                # [MODIFY] 6 处变量初始化（BC42030×6）
│   ├── ObjectModel\SensorData.vb                            # [MODIFY] 2 处变量初始化（BC42030×2：20/21）
│   ├── ObjectModel\SensorManager.vb:276                     # [MODIFY] stm 变量初始化（BC42108）
│   └── Resources\SensorsLocalizedMessages.Designer.vb       # [MODIFY] 移动 XML 注释块（BC42300）
├── Shell\
│   ├── Common\
│   │   ├── ShellException.vb                                # [MODIFY] 删除 <Serializable> 与序列化构造函数（SYSLIB0051）
│   │   ├── ShellObject.vb                                   # [MODIFY] parentShellItem 初始化（BC42030:312）；hashProvider 改用 MD5.Create()（SYSLIB0021:409）
│   │   ├── SearchCondition.vb:123                           # [MODIFY] subConditionObj 初始化（BC42030）
│   │   ├── SearchConditionFactory.vb                        # [MODIFY] 6 处变量初始化（BC42030×6：162/186/209/232/255/323）
│   │   ├── ShellLibrary.vb                                  # [MODIFY] 7 处变量初始化（BC42030×7：143/177/241/259/351/629/640）
│   │   ├── ShellObjectCollection.vb:54                      # [MODIFY] shellItemArray 初始化（BC42030）
│   │   ├── ShellObjectFactory.vb                            # [MODIFY] 3 处 nativeShellItem 初始化（BC42030×3：151/171/188）
│   │   └── ShellSearchFolder.vb                             # [MODIFY] 2 处变量初始化（BC42030×2：99/127）
│   ├── CommonFileDialogs\
│   │   ├── CommonFileDialog.vb                              # [MODIFY] 10 处变量初始化（BC42030×10）
│   │   ├── CommonFileDialogControlCollection.vb:85          # [MODIFY] groupBox 初始化（BC42030）
│   │   ├── CommonFileDialogTextBox.vb:99                    # [MODIFY] textValue 初始化（BC42030）
│   │   ├── CommonOpenFileDialog.vb                          # [MODIFY] 2 处 resultsArray 初始化（BC42030×2：142/157）
│   │   └── CommonSaveFileDialog.vb                          # [MODIFY] 3 处变量初始化（BC42030×3：203/229/240）
│   ├── ExplorerBrowser\CommonControlException.vb            # [MODIFY] 删除序列化构造函数（SYSLIB0051）
│   ├── Interop\Taskbar\TaskbarNativeMethods.vb:165          # [MODIFY] propStore 初始化（BC42030）
│   ├── KnownFolders\
│   │   ├── FoldersIdentifiers.vb:36                         # [MODIFY] folder 初始化（BC42030）
│   │   ├── FolderTypes.vb:220                               # [MODIFY] type 初始化（BC42030）
│   │   └── KnownFolderHelper.vb                             # [MODIFY] 5 处变量初始化（BC42030×5：21/36/57/74/111）
│   ├── PropertySystem\
│   │   ├── PropertySystemException.vb                       # [MODIFY] 删除序列化构造函数（SYSLIB0051）
│   │   ├── ShellProperty.vb                                 # [MODIFY] 2 处变量初始化（BC42030×2：59/280）
│   │   ├── ShellPropertyDescription.vb                      # [MODIFY] 2 处变量初始化（BC42030×2：208/218）
│   │   └── ShellPropertyFactory.vb:52                       # [MODIFY] ctor 初始化（BC42030）
│   ├── ShellObjectWatcher\
│   │   ├── ChangeNotifyEventManager.vb                      # [MODIFY] 3 处 del 初始化（BC42030×3：25/35/56）
│   │   ├── ChangeNotifyLock.vb                              # [MODIFY] 4 处变量初始化（BC42030×4：24/26/37/39）
│   │   ├── MessageListener.vb:141                           # [MODIFY] listener 初始化（BC42030）
│   │   └── MessageListenerFilter.vb:71                      # [MODIFY] action 初始化（BC42030）
│   ├── Taskbar\
│   │   ├── JumpList.vb                                      # [MODIFY] 6 处变量初始化（BC42030×6：128/289/337/353/360/489）
│   │   └── TabbedThumbnailManager.vb                        # [MODIFY] 2 处 thumbnail 初始化（BC42030×2：74/101）
│   └── Resources\ShellLocalizedMessages.Designer.vb         # [MODIFY] 移动 XML 注释块（BC42300）
└── ShellExtensions\
    ├── StorageStream.vb:171                                 # [MODIFY] stats 结构体初始化（BC42108）
    ├── ThumbnailProviders\ThumbnailProvider.vb              # [MODIFY] 3 处变量初始化（BC42030×3：32/35/38）
    └── Resources\ShellExtensionsLocalizedMessages.Designer.vb  # [MODIFY] 移动 XML 注释块（BC42300）
```

共涉及 **39 个文件**（35 个 `[MODIFY]`，无新增文件）。