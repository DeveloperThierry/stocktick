{ pkgs, ... }: {
  channel = "stable-23.11";
  packages = [
    pkgs.dotnet-sdk_8
  ];
  idx = {
    extensions = [
      "ms-dotnettools.csharp"
    ];
    previews = {
      
    };
  };
}