# EasyBlazor.DataCenter.Templates

[![EasyBlazor.DataCenter.Templates](https://img.shields.io/nuget/v/EasyBlazor.DataCenter.Templates.svg?color=green)](https://www.nuget.org/packages/EasyBlazor.DataCenter.Templates/)
![EasyBlazor.DataCenter.Templates](https://img.shields.io/nuget/dt/EasyBlazor.DataCenter.Templates.svg?color=red)
![EasyBlazor.DataCenter.Templates](https://img.shields.io/badge/License-MIT-blue)

## 简介

该项目是一个基于[Blazor](https://docs.microsoft.com/zh-cn/aspnet/core/blazor)和[Ant Design Blazor开源组件](https://ant-design-blazor.gitee.io/zh-CN/)，创建小型后台数据管理系统的模板。

模板基于.NET官方引擎：[dotnet/templating](https://github.com/dotnet/templating)。使用模板，可以很方便地创建解决方案，快速添加页面，减少开发干扰项，专注于完成业务需求。

## 推荐开发环境

Visual Studio 2022

## 技能栈准备

使用者应具备以下技能：

* C#
* HTML/CSS
* Blazor
* ASP.NET Core Web Api
* EntityFramework Core
* Ant Design Blazor开源组件（官网示例）

## 模板安装说明

请先检查是否安装了最新的.NET SDK。使用CLI工具执行命令：

```shell
dotnet new -i EasyBlazor.DataCenter.Templates
```

其他详情请参考微软官方文档《[dotnet new 自定义模板](https://docs.microsoft.com/zh-cn/dotnet/core/tools/custom-templates)》。

## 解决方案说明

模板生成的解决方案本质上是一个Hosted Blazor WebAssembly项目，前端Blazor，后端ASP.NET Core Web Api.

### 生成指令

```shell
dotnet new ebdc-sln -n Demo
```

### 数据库及ORM

默认数据库：SQL Server

ORM：EntityFramework Core

如果使用其他数据库，自己修改和添加依赖包支持即可。

### 第三方开源库依赖

前端引用模块：

* AntDesign
* Blazor.LocalStorage

后端引用模块：

* AutoMapper
* Linq2db

### 启动前说明

1. 请先检查是否已安装解决方案必需的.NET SDK版本
2. 使用`dotnet new`指令生成的解决方案，初始项目应可以直接编译通过
3. 填写`appsettings.json`的参数，主要是数据库连接字符串和JWT相关配置。否则启动会抛出异常
4. 使用EF Core工具生成数据库实体类及DbContext相关代码（具体操作可参阅微软官方文档）。解决方案自带了`Admin`实体类和一个默认的`DbContext`， 只是为了展示管理员登录模块的实现
5. 按需修改Client项目`wwwroot/uidata`文件夹中的所有json文件配置。这些json文件定义了前端UI展示的内容

*注意：初始解决方案不能面面俱到，开发者可以按照自身需求升级第三方依赖包、修改数据库支持、修改代码、添加配置等等*

## 模块生成说明

模块代码生成模板包括：

* 前端页面及Http请求代码
* 后端接口
* 其他模型类（FormData/ViewModel/ListItem/Request等）

### 生成指令

```shell
dotnet new ebdc-mod -n Demo -m Book

dotnet new ebdc-mod -n Demo --mod Book
```

### 设计说明

本模板的结构主要包含：

* 搜索列表页面，上方为搜索表单，下方为结果列表
* 详情页面，上方为不可更改的基础信息（Description组件），中间为可变信息（Form组件），最下方为危险操作区域（Popconfirm组件）
* 前端请求接口的BackendService
* API Controller

创建一个模块后，编译无法通过是正常的，使用者需要根据错误提示逐个补充或修改代码。

为了方便示例，生成的代码会默认实体类包含`Id`、`Name`、`CreateTime`三个字段。请开发者按需自行修改。

考虑到个人实际应用的频率，生成的接口不包含创建实体的POST请求，使用者可以按自己的实际需求自行添加。前端负责HTTP请求的`BackendService`同理，另外默认会将代码生成为一份单独的partial class文件，如果不需要，可以自行将代码复制到一起然后删除生成的`.cs`文件。

**开发者可以尝试任意Ant Design组件来修改和完善生成的页面，模板只负责快速生成页面的基本结构及提供基本的示例代码。**

## 版本说明

A.B.C.D

A：跟随解决方案的.NET版本

B/C：跟随Ant Design Blazor开源组件包的主要/次要版本

D：补丁版本

例如：模板生成的项目SDK版本为`.NET 6`，AntDesign开源组件包的版本为`0.11.0`，模板经历了3次修改（当前补丁号为3），则当前发布的版本为`6.0.11.4`
