# 上游、许可证与分发

## 项目来源

- 上游项目：[BeyondDimension/SteamTools](https://github.com/BeyondDimension/SteamTools)
- 基线版本：[2.8.6](https://github.com/BeyondDimension/SteamTools/releases/tag/2.8.6)
- 基线提交：`6a5f8db69f92a98a0503234f3b34433e2924c1c1`
- 本项目性质：非官方二次发行，不代表上游作者或官方发行渠道

建议保留 `upstream` Git remote 指向官方仓库，便于核对历史和安全更新。

## 主仓库许可证

根目录 `LICENSE` 为 GNU General Public License version 3。分发源码或二进制前，应完整阅读许可证原文，
保留许可证、版权和修改说明，并履行适用于实际分发方式的源代码提供义务。

本文只是项目维护说明，不构成法律意见。

## 第三方依赖

`references/` 中包含多个 Git 子模块；各子模块可能采用不同许可证。
分发构建产物前，应逐项核对锁定版本及其许可证、NOTICE、署名和再分发条件。

## 品牌与发布说明

发布页、安装包和应用内说明应明确标注“非官方二次发行”，避免让用户误认为构建产物来自 Watt Toolkit 官方。
不要复用上游私有签名、发布令牌或官方发布身份。
