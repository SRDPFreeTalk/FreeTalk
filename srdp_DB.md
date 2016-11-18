# Freetalk数据库(oucfreetalk)

## 数据库总览

|数据库名|作用|
|:------------|:---------|
|students|存储学生信息|
|posts|存储帖子，仅帖子头|

## 数据库详细介绍

### students(学生信息)

|列名|数据类型|是否可空|作用|
|:---------|:----------|:----------|:-----------|
|id|nchar(11)|否|学生的学号，作为唯一标识|
|name|nvarchar(50)|否|学生的真实姓名|
|ifname|bit|否|是否显示真实姓名|
|nikename|nvarchar(50)|否|在论坛中显示的昵称|
|pic|nvarchar(MAX)|否|用户在论坛中的头像|
|password|nchar(128)|否|使用hash加密算法得到固定长度的字符串。|
|sex|bit|否|用户的性别|
|ifsex|bit|否|是否显示用户的性别|
|birth|date|否|用户的出生年月日|
|ifbirth|bit|否|是否显示用户的出生年月日|
|year|nchar(4)|否|用户的入学年份|
|email|nvarchar(100)|是|用户的邮箱，用来绑定|
|ifemail|bit|否|是否显示用户的邮箱|
|mobile|nchar(11)|是|用户的手机号，用来绑定|
|ifmobile|bit|否|是否显示用户的手机号|
|exp|int|否|用户的经验值，由函数计算等级|
|family|nvcarchar(100)|是|用户的家乡|

### posts表
|列名|数据类型|是否可空|作用|
|:---------|:----------|:----------|:-----------|
|id|int|否|主键,作为唯一标识|
|title|nvarchar(100)|否|帖子的标题|
|createtime|datetime|否|帖子的创建时间|
|updatetime|datetime|否|帖子的最后回复时间|
|realbody|int|否|帖子的楼层和回复总数（包括抽取的楼）|
|body|int|否|帖子的楼层和回复现存数(不包括已抽取的楼数)|
|owner|nchar(11)|否|帖子的创建人id,关联主键|
|contentext|nvarchar(max)|否|一楼的内容页|
