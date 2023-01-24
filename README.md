# Hollow

[Hollow](https://github.com/xiosec/Hollow) is a tool for implementing the [process hollowing](https://attack.mitre.org/techniques/T1055/012/) technique.

[![xiosec - Hollow](https://img.shields.io/static/v1?label=xiosec&message=Hollow&color=blue&logo=github)](https://github.com/xiosec/Hollow)
[![stars - Hollow](https://img.shields.io/github/stars/xiosec/Hollow?style=social)](https://github.com/xiosec/Hollow)
[![forks - Hollow](https://img.shields.io/github/forks/xiosec/Hollow?style=social)](https://github.com/xiosec/Hollow) [![GitHub release](https://img.shields.io/github/release/xiosec/Hollow?include_prereleases=&sort=semver)](https://github.com/xiosec/Hollow/releases/)
[![License](https://img.shields.io/badge/License-MIT-blue)](#license)
[![issues - Hollow](https://img.shields.io/github/issues/xiosec/Hollow)](https://github.com/xiosec/Hollow/issues)

> Process hollowing is a method of executing arbitrary code in the address space of a separate live process.

Process hollowing is commonly performed by creating a process in a suspended state then unmapping/hollowing its memory, which can then be replaced with malicious code. A victim process can be created with native Windows API calls such as `CreateProcess`, which includes a flag to suspend the processes primary thread. At this point the process can be unmapped using APIs calls such as `ZwUnmapViewOfSection` or `NtUnmapViewOfSection` before being written to, realigned to the injected code, and resumed via `VirtualAllocEx`, `WriteProcessMemory`, `SetThreadContext`, then `ResumeThread` respectively.

This is very similar to `Thread Local Storage` but creates a new process rather than targeting an existing process. This behavior will likely not result in elevated privileges since the injected process was spawned from (and thus inherits the security context) of the injecting process. However, execution via process hollowing may also evade detection from security products since the execution is masked under a legitimate process.

## Build

* To compile the project you must install `.NET Core` or use `Visual Studio`

```
$ dotnet build
```
* packages

```
$ dotnet list package

Project 'Hollow' has the following package references
   [netcoreapp3.1]:
   Top-level Package        Requested   Resolved
   > CommandLineParser      2.9.1       2.9.1
```
## Usage

> Help

```
$ Hollow.exe --help

Hollow 1.0.0
Copyright (C) 2022 Hollow

  -v, --vprocess    Required. The victim's process file pathway

  -m, --mprocess    Required. The malicious's process file pathway

  --help            Display this help screen.

  --version         Display version information.
```

> Example

```

$ Hollow.exe -v "C:\Windows\System32\notepad.exe" -m "C:\Windows\System32\cmd.exe"

 _  _       _  _
| || | ___ | || | ___  _ __ __
| __ |/ _ \| || |/ _ \ \ V  V /
|_||_|\___/|_||_|\___/  \_/\_/

Hollow: 1.0.0
repository: github.com/xiosec/Hollow
twitter: twitter.com/xiosec

[+] Successfully created the process
        \___[*] Process Id: 9999
        \___[*] Thread Id: 9999
        \___[*] ImageBase Address: 0xffffffffff
[+] Copy Headers and Sections
[+] Number of sections: 7
...

```
* In this example the process is the victim of `Notepad` and the malicious process of `CMD`

> 
## License

Released under [MIT](/LICENSE) by [@xiosec](https://github.com/xiosec).
