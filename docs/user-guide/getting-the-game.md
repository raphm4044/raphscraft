# Getting the game
## Prerequisites
If you actually want to play the game, you would need the following system configuration:
- An OS supported by the .NET runtime
- A Vulkan 1.2-compliant GPU with Vulkan 1.2-capable GPU drivers.

## From binary builds
> [!NOTE]
> There are currently no available binary builds of Raph's Craft. You must compile it from source.

## From source
### Prerequisites
- An operating system that supports .NET (Linux, FreeBSD, macOS, Windows, etc..)
- .NET SDK (9.0.0 or later) with the "dotnet" executable being in the PATH scope.

### Steps
After you cloned the repo, you need to build it.

#### [Using the terminal](#tab/terminal)
- Open a terminal in the project's directory (where `Raphscraft.csproj` is contained)
- Build the project: `dotnet build`

After following those steps, there will be a binary build in `bin/Debug/net9.0`. 
You can launch the "Raphscraft" executable and see the game in action!

#### [Using an IDE](#tab/ide)
- Open the solution in your favourite IDE (Rider, Visual Studio, etc...)
- Build the project
  - Rider: Build > Build Solution
  - Visual Studio: Build > Build (or Ctrl+Shift+B)

After following those steps, there will be a binary build in `bin/Debug/net9.0`.
You can launch the "Raphscraft" executable / click the "Run" button on your IDE, and see the game in action!