Get-ChildItem .\ -include bin,obj,bld,Backup,_UpgradeReport_Files,Debug,Release,ipch,TestResults,PortabilityAnalysis.html -Recurse | foreach ($_) { remove-item $_.fullname -Force -Recurse }