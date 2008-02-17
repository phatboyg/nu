<?xml version="1.0" encoding="utf-8" ?>
<project>
  <folders>
    <folder path=".${PathSeparator}src" />
    <folder path=".${PathSeparator}src${PathSeparator}${ProjectName}" />
    <folder path=".${PathSeparator}src${PathSeparator}${ProjectName}.Tests" />
    <folder path=".${PathSeparator}lib"  />
    <folder path=".${PathSeparator}tools" />
    <folder path=".${PathSeparator}docs" />
  </folders>
  <files>
    <file source=".${PathSeparator}Default.sln" destination="${PathSeparator}src${PathSeparator}${ProjectName}.sln" />
    <file source=".${PathSeparator}Default.csproj" destination="/src/${ProjectName}${PathSeparator}${ProjectName}.csproj" />
    <file source=".${PathSeparator}Default.Tests.csproj" destination="/src/${ProjectName}.Tests${PathSeparator}${ProjectName}.Tests.csproj" />
  </files>
  <packages>
    <package name="" />
    <package name="" />
  </packages>
</project>