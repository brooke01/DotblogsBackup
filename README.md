# DotblogsBackup
這是一個個人在使用的點部落格文章備份程式(爬蟲程式)，由於個人文章已搬家，所以現已停止維護了。

<p>以下說明程式架構</p>

一、一個「點部落格文章備份程式」主要會完成三個動作

1、備份 css 樣式。

2、備份文章內容。

3、備份文章圖片。

<pre style="margin:0em; overflow:auto; background-color:#ffffff;"><code style="font-family:Consolas,&quot;Courier New&quot;,Courier,Monospace; font-size:10pt; color:#000000;"><span style="color:#0000ff;">static</span> <span style="color:#0000ff;">void</span> Main(<span style="color:#0000ff;">string</span>[] args)
{
    Console.WriteLine(<span style="color:#a31515;">"ToBackupStyleSheet theme-light.css ..."</span>);
    ToBackupStyleSheet();

    Console.WriteLine(<span style="color:#a31515;">"ToBackupArticle starting...please waiting A few minutes"</span>);
    ToBackupArticle();

    Console.WriteLine(<span style="color:#a31515;">"ToBackupPicture starting...please waiting A few minutes"</span>);
    ToBackupPicture();

    Console.WriteLine(<span style="color:#a31515;">"All finish"</span>);
    Console.WriteLine(@<span style="color:#a31515;">"please push "</span><span style="color:#a31515;">"enter"</span><span style="color:#a31515;">" button to exit..."</span>);
    Console.ReadLine();
}
</code></pre>

