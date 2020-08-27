Write-Output "Fuck JavaScript";
Write-Output "tsc is running!";
Set-Location "ts";
$fs = Get-ChildItem; # 获取当前目录内容
foreach ($f in $fs) {
  if ($f.Extension.ToLower().Equals(".ts")) {
    tsc --outFile "../js/$($f.Name.SubString(0,$f.Name.Length-$f.Extension.Length)).js" $f.Name
  }
}
Write-Output "tsc is done!"