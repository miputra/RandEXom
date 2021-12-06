**What is RandEXom?**

RandEXom basically a library to make randomization easier. There are so many ways you can do to do randomizing in programming, and RandEXom trying to archieve those methods in a single library, either by using an already made algorithm or a new algorithm, so we can do randomizing with whatever method we want.


***


## Requirement

To use this library, you need at least:
.Net Framework 4.6.1


***


## How to use

Note: Every IDE has a different way to add .dll as a reference. 

If you use the visual studio on your project:

1. In Solution Explorer, right-click your project, then click add reference
2. On the bottom left window, click the browser button, and choose RandEXom.dll
3. Now you can using RandEXom library in your project


***

## Structure

RandEXom library consists of several modules and classes

* RandEXom   
  * RandomLib
    * NetRandom
  * SeedLib
    * SeedR
    * IterativeSeedR
    * IterativeSeedRCustom
  * FrameWork
    * DistanceR
    * GachaR
    * GachaRBatched
    * PongR
  * Interface
    * IGachaR
    * IRandomR
    * ISeedR
  * Utility
    * TypeR
