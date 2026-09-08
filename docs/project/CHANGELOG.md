# 修改记录

本文件只记录该二次发行相对 Watt Toolkit 官方 `2.8.6` 的差异。

## 未发布

### 功能变更

- 默认且强制关闭应用内广告请求与展示；
- 保留上游广告实现，以集中式编译期开关控制，方便审阅与恢复；
- 保留登录、赞助身份、捐助列表等非广告功能。

### 兼容性

- 显式调用 `Enumerable.Reverse`，避免使用较新 .NET SDK 编译时数组原地 `Reverse` API 与 LINQ 扩展解析冲突；
  该机械调整覆盖 AES 参数序列化、Hosts 文件倒序检查以及对应单元测试，不改变原有枚举顺序或业务结果。

### 验证

- `ST.Client` 的 `net6.0` Release 目标构建成功；
- `ST.Client.UnitTest` 共 11 项测试全部通过；
- 完整桌面 App 在本机 .NET SDK 9.0.309 下止于历史 Windows SDK 投影的 `NETSDK1148` 环境兼容错误，
  仍需使用上游 CI 指定的 .NET SDK 6.0.x 做最终 App 构建与运行时界面检查。
- 新增固定 .NET SDK 6.0.428 的 Windows Actions 构建，生成无签名、自包含的 `win-x64` Release
  测试 Artifact，并在产物中记录源码提交与主程序 SHA-256。

### 文档

- 在中英文 README 中加入非官方二次发行声明；
- 在中英文 README 中加入共用 `ktclat-steamtool` 计数键的 Moe Counter 累计访问计数；
- 新增广告实现审阅、构建说明、上游与许可证说明。

### 仓库维护

- 移除上游的每日 Dependabot 版本更新配置，避免自动创建依赖更新分支；依赖升级改为经过兼容性审阅后人工提交。
- 移除指向原作者 Gitee 仓库且依赖其私钥的自动同步工作流；Xamarin 工作流改为仅手动触发，避免桌面端
  提交产生无关的旧移动端构建。

### 基线

- 上游仓库：`BeyondDimension/SteamTools`；
- 上游标签：`2.8.6`；
- 上游提交：`6a5f8db69f92a98a0503234f3b34433e2924c1c1`。
