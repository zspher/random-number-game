{
  description = "A basic flake with a shell";
  inputs.nixpkgs.url = "github:NixOS/nixpkgs/nixpkgs-unstable";
  inputs.flake-utils.url = "github:numtide/flake-utils";

  outputs = {
    self,
    nixpkgs,
    flake-utils,
  }:
    flake-utils.lib.eachDefaultSystem (system: let
      pkgs = nixpkgs.legacyPackages.${system};
      xorgDeps = with pkgs.xorg; [libX11 libICE libSM];
    in {
      devShells.default = pkgs.mkShell {
        packages = with pkgs; [
          (with dotnetCorePackages;
            combinePackages [
              sdk_6_0
              sdk_8_0
            ])
          # formatter
          csharpier

          # lsp
          omnisharp-roslyn

          # debugger
          netcoredbg
        ];

        nativeBuildInputs = with pkgs; [gnumake];

        buildInputs = with pkgs; [dotnet-sdk_8 fontconfig icu] ++ xorgDeps;

        shellHook = with pkgs; ''
          export DOTNET_ROOT=${dotnet-sdk_8}
          export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:${lib.makeLibraryPath ([fontconfig icu] ++ xorgDeps)}
        '';
      };
    });
}
