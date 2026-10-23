{ pkgs ? import <nixpkgs> {} }:

pkgs.mkShell {
  # nativeBuildInputs contains tools you want available in your $PATH
  nativeBuildInputs = with pkgs; [
    dotnet-sdk_8
    mono
  ];

}
