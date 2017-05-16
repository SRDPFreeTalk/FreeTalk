# PostsController介绍

## 总体介绍

本控制器包含了跟帖子功能有关的接口

## 接口总览

|接口|方法|数据|返回|作用|
|:-----:|:-----|:-----|:-----|:-----|
|/api/Posts/addPost|Post|Josn|Json|用户添加一个新的主题贴|
|/api/Posts/addcomments|Post|Json|Json|添加一个楼层|
|/api/Posts/getPosts|Get|Json|Json|获取指定页的帖子|
|/api/Posts/getPostsPage|Get|Json|Json|获取帖子总页数|
|/api/Posts/addClass|Post|Json|Json|添加一个板块|
|/api/Posts/editorClass|Post|Json|Json|修改一个板块的名字|
|/api/Posts/deleteClass|Post|Json|Json|删除一个板块|
|/api/Posts/getClasses|Get|null|Json|获取板块|


## 接口详情

### /api/Posts/addPost

方法 POST

数据 
```
{
    title:"aaa",    //帖子名 
    context:"aasdasad",    //帖子头
    pclass: 1     //帖子版块
}

```

### /api/Posts/addcomments

```
{
    
}
```

### /api/Posts/getPosts

### /api/Posts/getPostsPage

### /api/Posts/addClass

### /api/Posts/editorClass

### /api/Posts/deleteClass

### /api/Posts/getClasses

