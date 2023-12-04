#!/usr/bin/env pwsh
#Requires -Version 7.2

param($Configuration = "Release")

. $PSScriptRoot\Common.ps1

Build-Project $Configuration YR Universal net7.0
if ($IsWindows) {
    Build-Project $Configuration YR Windows net7.0-windows
}