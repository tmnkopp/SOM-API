## UPDATE AspNetCoreModuleV2

$base = 'C:\inetpub\wwwroot\'
$builds = $base + 'builds\' 
$releasefrom = "C:\inetpub\wwwroot\somapi_Release\"
$releaseto = $base + "somapi\"
$temp = $base + "temp\" 
$build_dest = $builds + 'somapi' + "-" + $(Get-Date -format 'MM-dd-yyyy-HH-mm-ss') 

$webconf = $releasefrom + "web.config" 
$content = Get-Content -Path $webconf 
$content = $content.Replace("AspNetCoreModule""","AspNetCoreModuleV2""")
Set-Content -Path $webconf  -Value $content 

Copy-Item -Path ($releasefrom) -Destination ($build_dest) -Recurse
Remove-item ( $temp + "*" ) -recurse -force
Copy-Item -Path ($releaseto + "*config*" ) -Destination ($temp) 
Copy-Item -Path ($releaseto + "*appsettings*" ) -Destination ($temp) 

$HasFiles = Test-Path -Path $releasefrom* 
if( $HasFiles )
{
    Remove-item ( $releaseto + "*" ) -recurse -force
    Copy-Item   -Path ($releasefrom + "*") -Destination (  $releaseto ) -recurse  -force
    Copy-Item   -Path ($temp + "*") -Destination (  $releaseto ) -recurse  -force  
}



