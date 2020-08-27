# Shimakaze Docs
<div style="margin:0 -10px 0 0;padding:20px;background-color:rgba(60,60,90,.6);border-radius:10px;">
<b>注意</b>:  

`Shimakaze Docs`是分散的文档, 每个项目各一个, 其由[`docsify`](//docsify.js.org/)强力驱动
</div>

# Shimakaze.Struct.Csf
这是一个为CNC系列的Csf文档提供简单数据结构的项目  
使用这个库可以更加方便的处理Csf文件  
这个文档咕咕咕了


这个类库包含的公开内容:  
CsfValue  CsfValueHelper  
CsfLabel  CsfLabelHelper  
CsfHead  CsfHeadHelper
CsfClass  

简述: 
`CsfValue`是一个CS发字符串  
`CsfLabel`是一个标签 一个标签可能包含多个字符串  
`CsfHead`是CSF文件的文件头 这是一个结构 长度固定是24byte  
各种Helper: 这是一个放扩展方法的类 里面只有一个DeparseAsync扩展方法 用于将对象转换为CSF标准内容  
`CsfClass`这是一个CSF文件中不存在的结构 这个不存在的结构会被拿来做CSF文件编辑器的标签分类 