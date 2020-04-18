
$base = 'C:\temp\$testing\'
$builds = 'C:\temp\$testing\builds\' 
New-Item -Path $base -Name "temp" -ItemType "directory" -Force
$temp=$base+"temp\"
Foreach($dir in dir $base ){
      
      if( $dir.name -match '(.*)_Release'  ){
            $src_release = $base + $dir.name + "\"  
            $dest = $base + $Matches[1] + "\"   
            $build_dest = $builds + $Matches[1] + "-" + $(Get-Date -format 'MM-dd-yyyy-HH-mm-ss') 

            Copy-Item -Path ($src_release) -Destination ($build_dest) -Recurse

            Copy-Item -Path ($dest + "*config*" ) -Destination ($temp) 
            Copy-Item -Path ($dest + "*appsettings*" ) -Destination ($temp) 
            
            Remove-item ( $dest + "*" ) -recurse -force
            Copy-Item   -Path ($src_release + "*") -Destination ( $dest ) -recurse  -force
            Copy-Item   -Path ($temp + "*") -Destination ( $dest ) -recurse  -force 

		Remove-item ( $temp + "*" ) -recurse -force 
      }
} 