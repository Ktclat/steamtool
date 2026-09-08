# 项目文档

本仓库是 Watt Toolkit（原 Steam++）官方 `2.8.6` 源码的非官方二次发行。
基线为上游标签 [`2.8.6`](https://github.com/BeyondDimension/SteamTools/releases/tag/2.8.6)，
对应提交 `6a5f8db69f92a98a0503234f3b34433e2924c1c1`。

当前二次发行遵循两个原则：尽量缩小与上游的代码差异，以及保留原项目的署名、许可证和可追溯性。

## 文档索引

- [广告策略与实现审阅](./architecture/advertising-policy.md)：说明上游广告链路、修改点与恢复方法。
- [构建与验证](./development/build.md)：本地初始化、构建、测试及发布注意事项。
- [修改记录](./project/CHANGELOG.md)：记录相对官方 `2.8.6` 的功能性差异。
- [上游、许可证与分发](./project/upstream-and-license.md)：列出项目来源和再分发注意事项。

## 当前范围

除关闭应用内广告请求与展示、三处不改变语义的新版 SDK 编译兼容调整以及补充仓库文档外，
当前没有主动改变上游业务功能。
项目中的登录、赞助身份、捐助页面、广告模型和广告视图仍保留，便于审计和按需恢复。
