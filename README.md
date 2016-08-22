#QiniuLab 七牛云存储实验室

##简介

最新版本下载[QiniuLab-v1.2](http://devtools.qiniu.com/QiniuLab-v1.2.2.zip)

![01](docs/imgs/01.png)

(1)功能菜单(左侧工具栏)

文件上传、文件下载、资源管理、持久化、工具、设置

(2)快捷链接(右上角文本链接)

七牛云存储官网、开发者文档、七牛云相关问答(Segment Fault)、用户管理后台(portal)

(3)快捷导航

历史菜单操作会被记录，点击导航按钮或者下拉按钮可以快速定位

![02](docs/imgs/02.png)

##设置

账号设置，设置AK&SK

![03](docs/imgs/03.png)

其中AK&SK可以在portal的“密钥管理”页面找到

![03x](docs/imgs/03x.png)

##文件上传

点击左边栏“文件上传”-->配置“上传策略”-->“生成Token”-->“浏览并上传文件”

![04](docs/imgs/04.png)

![05](docs/imgs/05.png)

处理结果(回复消息查看)

![06_1](docs/imgs/06_1.png)

除了普通方式之外还可以按照其他方式查看：

![06_2](docs/imgs/06_2.png)

![06_3](docs/imgs/06_3.png)

##文件下载

私有空间文件受到保护(未被授权)无法直接下载，如下图：

![07](docs/imgs/07.png)

为了得到授权的访问(可下载)，可以依照以下流程操作生成授权的下载外链：

![08](docs/imgs/08.png)

用已授权的外链访问（请在有效期内执行），可以正常下载，如下图：

![09](docs/imgs/09.png)

##资源管理

已集成几乎全部功能，如stat, copy, move, delete, batch等

![10](docs/imgs/10.png)

某些情形下回复“内容”为空

![11_1](docs/imgs/11_1.png)

可以切换查看RAW“信息”，如下图：

![11_2](docs/imgs/11_2.png)

##批处理

格式为：`op=(opString1)&op=(opString2)&op=…`

![12](docs/imgs/12.png)

##持久化

持久化(pfop)可以参考[官方文档介绍](http://developer.qiniu.com/article/index.html#fop-official)

![13](docs/imgs/13.png)

持久化结果查询

![14](docs/imgs/14.png)

##工具

Base64编码/解码

![15](docs/imgs/15.png)

QEtag计算和CRC32校验码

![16](docs/imgs/16.png)

##意见&帮助

如果你有任何的意见，可以通过提 issue，我们来讨论。
