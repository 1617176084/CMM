# CMM
CMM语言解释器-C#-WindowsPhone版本

![img](./1.png){:height="50%" width="50%"}
 <img src="./1.png" style="zoom:20%" />

cmm是c的一个子集，保留字只有如下几个

if else while read write int real

特殊符号有如下几个

+ - * / = < == <> ( ) ; { } [ ] /* */
标识符：由数字，字母或下划线组成的字符串，且不能使关键字，第一个字母不能是数字

如果了解c很容易明白上面的是什么意思，也会明白cmm其实有的东西并不多，所以做cmm解释器相对来说比较简单。



上面的特殊符号实际上比较少，我个人实现的时候还对> >= <=等做了相关的支持，当然，原理上都是一样。

