MSIX Permission Check
=====================

This is a tool which can be used to check file registry r/w permissions in MSIX.

## Build

1. Build MSIX-PCheck.exe

2. Replace the placeholder in `Packaging\VFS`

3. Use your application icon to replace assets in `Packaging\Assets`

4. Update `Packaging\AppxManifest.xml`

5. Use `makeappx.exe` to pack `Packaging` then sign with your digital signature.

## Usage

```
# print usage
> info

# enable debug info
> debug
> ...

# file operations
> file|exec\read\new\delete\write|path

# registry operations
> reg|get\set\new\delete|root|key|value|data

# run testing
> test

```