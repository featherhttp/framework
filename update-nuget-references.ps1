$version = $(Get-Content NUGET_VERSION)
if (!$version) { $version = "Default" }
if ($version -ne "Default") {
    $nuspecXml = [xml](Get-Content ./template/FeatherHttpTemplate.nuspec)
    $nuspecXml.selectNodes("//version") | ForEach-Object {
      $_."#text" = $version.ToString()
      Write-Host "Updated template nuspec"
    }
    $nuspecXml.save("./template/FeatherHttpTemplate.nuspec")
    $csprojXml = [xml](Get-Content ./template/content/featherhttp-template.csproj)
    $csprojXml.selectNodes("//PackageReference") | ForEach-Object {
      $_.Version = $version.ToString()
      Write-Host "Updated template csproj"
    }
    $csprojXml.save("./template/content/featherhttp-template.csproj")
}