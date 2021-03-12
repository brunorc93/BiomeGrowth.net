# Biome Growth Machine

This was coded with C# initially for Unity3D and then adapted for C#.net due to low dependency on Unity and ease of showing the results. 
It can be compiled with .net sdk 5 using ``dotnet build`` or ``dotnet run`` in the my-app folder through the command line.

It builds a command line application that opens up a window showing the generation process if wanted (this causes it to run at a slower speed to be able to showcase the entire process).

example .gif of generation process:

<div style="display: inline-block">
    <img style="float: left;" src="examples/gifs/005f.gif?raw=true" width="200" height="200" alt="Biome Growth Process">
    <img style="float: left;" src="examples/blank.png?raw=true" width="50" height="200" alt="blank">
    <img style="float: left;" src="examples/gifs/006f.gif?raw=true" width="200" height="200" alt="Biome Growth Process">
    <img style="float: left;" src="examples/blank.png?raw=true" width="50" height="200" alt="blank">
    <img style="float: left;" src="examples/gifs/007f.gif?raw=true" width="200" height="200" alt="Biome Growth Process">
</div>

This generates biomes through the RGB channels of a bitmap using the R channel as the key value as it is different for each node, where B and G values are just random (having a slight chance of having 2 nodes with same B or G value). The results can be seen in the following images:

<div style="display: inline-block">
  <img style="float: left;" src="examples/000_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/001_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/002_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/003_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/004_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/005_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/006_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/007_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/008_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/009_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/010_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/011_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/012_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/013_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/014_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/015_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/016_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/017_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/018_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/019_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/020_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/021_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/022_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/023_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/024_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/025_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/026_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/027_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/028_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/029_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/030_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/031_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
  <img style="float: left;" src="examples/032_.png?raw=true" width="100" height="100" alt="Example of grown biomes">
</div>