# Freetalk数据库

##数据库总览

|数据库名|作用|
|:------------|:---------|
|students|存储学生信息|
|postclass|存储帖子分类|
|posts|帖子头|
|postc|帖子的普通楼层|
|postreply|帖子的回复|
|friends|好友管理|
|blogs|日志|
|comment|评论（日志）|
|blogreply|日志评论回复|
|accountaccess|用户权限表|

##数据库详细介绍

###students(学生信息)

|列名|数据类型|是否可空|作用|
|:---------|:----------|:----------|:-----------|
|id|nchar(11)|否|学生的学号，作为唯一标识|
|name|nvarchar(50)|否|学生的真实姓名|
|ifname|bit|否|姓名是否显示|
|nikename|nvarchar(50)|否|在论坛中显示的昵称|
|pic|nvarchar(MAX)|否|用户在论坛中的头像|
|password|nchar(128)|否|使用hash加密算法得到固定长度的字符串。|
|sex|bit|否|用户的性别|
|ifsex|bit|否|性别是否显示|
|birth|date|否|用户的出生年月|
|ifbirth|bit|否|生日是否显示|
|year|nchar(4)|否|用户的入学年份|
|email|nvarchar(100)|是|用户的邮箱，用来绑定|
|ifemail|bit|否|邮箱是否显示|
|mobile|nchar(11)|是|用户的手机号，用来绑定|
|ifmobile|bit|否|手机号是否显示|
|exp|int|否|经验，初始值为0|
|family|nvarchar(100)|否|家乡，字符串表示即可|

###postclass(帖子分类)

|列名|数据类型|是否可空|作用|
|:---------|:----------|:----------|:-----------|
|id|int|否|主键，作为唯一标识|
|name|nvarchar(50)|否|分类的名称|

###posts(帖子信息)

|列名|数据类型|是否可空|作用|
|:---------|:----------|:----------|:-----------|
|id|int|否|主键，作为唯一标识|
|title|nvarchar(100)|否|标题，能够显示帖子内容的简短概括|
|createtime|datetime|否|帖子创建时间|
|updatetime|datetime|否|帖子最后回复的时间|
|realbody|int|否|当前的帖子数（不包含已删除或屏蔽的楼层）|
|body|int|否|当前的帖子总数（包含已屏蔽或删除的楼层，为了计算出下一楼）|
|owner|nchar(11)|否|帖子的创建者|
|contentext|nvarchar(MAX)|否|帖子一楼内容，由添加帖子的时候一起添加|
|ownclass|int|否|所在的分区|

###postc(帖子主楼层)

|列名|数据类型|是否可空|作用|
|:---------|:----------|:----------|:-----------|
|id|int|否|主键，作为唯一标识|
|postlocation|int|否|该跟帖所处的楼层数的|
|owner|ncahr(11)|否|跟帖人|
|createtime|datetime|否|跟帖时间|
|body|nvarchar(50)|否|跟帖内容|
|ownpost|int|否|该跟帖所属的帖子|

###postreply(帖子回复)

|列名|数据类型|是否可空|作用|
|:---------|:----------|:----------|:-----------|
|id|int|否|主键，作为唯一标识|
|createtime|datetime|否|创建时间|
|owner|nchar(11)|否|回复的发起者|
|replyto|ncahr(11)|否|被回复人|
|ownlocation|int|否|回复所在的楼层|
|contenttext|nvarchar(max)|否|回复内容|

###friends(好友表)
|列名|数据类型|是否可空|作用|
|:---------|:----------|:----------|:-----------|
|focus|nchar(11)|否|主键之一关注者|
|befocus|nchar(11)|否|主键之一被关注者|
|createtime|datetime|否|关注关系创建时间|

###blogs(日志)
|列名|数据类型|是否可空|作用|
|:---------|:----------|:----------|:-----------|
|id|int|否|主键，唯一标识|
|title|nvarchar(100)|否|日志的标题|
|body|nvarchar(max)|否|日志的主体|
|createtime|datetime|否|日志的创建时间|
|updatetime|datetime|否|日志最后修改的时间|
|owner|nchar(11)|否|日志的创建者|

###comment(评论)
|列名|数据类型|是否可空|作用|
|:---------|:----------|:----------|:-----------|
|id|int|否|主键唯一标识|
|owner|nchar(11)|否|评论人|
|createtime|datetime|否|评论创建时间|
|body|nvarchar(max)|否|评论内容|
|ownlocation|int|否|评论的日志|

### blogreply(日志评论回复)
|列名|数据类型|是否可空|作用|
|:---------|:----------|:----------|:-----------|
|id|int|否|主键唯一标识|
|owner|nchar(11)|否|回复人|
|replyto|nchar(11)|否|被回复人|
|createtime|datetime|否|创建时间|
|ownlocation|int|否|回复的评论|
|body|nvarchar(max)|否|回复内容|

### accountaccess(用户权限管理)

|列名|数据类型|是否可空|作用|
|:---------|:----------|:----------|:-----------|
|id|int|否|主键唯一标识|
|studentid|nchar(11)|否|权限人员|
|classid|int|否|权限  >0 板块的id  -1 所有版块管理员  -2 管理员的管理员 -1-2可同时拥有|
|createtime|datetime|否|创建时间|
