<p align="center">
   Custom Samples & Patches Loader (CSPL) is a custom loader which loads all your samples & helm patches into the VR game Virtuoso
</p>

## Disclaimer
This mod will prevent you from uploading your sessions to the community. if you try it anyways it will upload them to your cloud storage instead. Out of respect to the developers I added this until a proper solution is found. Normal saves and cloud upload work fine.

## Requirements
- .NET Framework 4.8

## How to add custom helm patches
- copy your .helm files into the ``<GAME_DIRECTORY>/import/patches`` folder  (IMPORTANT: must be in helm format!)

## How to add custom samples
- copy your .wav sample files into the ``<GAME_DIRECTORY>/import/samples/<GROUPNAME>`` folder
- (GROUPNAMES: ``Bass Drums``, ``Car Doors``, ``Crashes``, ``Effects``, ``Hi-hats``, ``Rides``, ``Snares`` and ``Toms``)

## How to use
- just start the game
- empads: depending on which folder you dropped the sample in, it will show up on that empad group alongside the original ones
- other instruments: in the menu at the bottom left you will find your custom patches alongside the original ones
- enjoy!

## Contributing
1. Fork it (<https://github.com/v0idp/CSPL/fork>)
2. Create your feature branch (`git checkout -b feature/fooBar`)
3. Commit your changes (`git commit -am 'Add some fooBar'`)
4. Push to the branch (`git push origin feature/fooBar`)
5. Create a new Pull Request
