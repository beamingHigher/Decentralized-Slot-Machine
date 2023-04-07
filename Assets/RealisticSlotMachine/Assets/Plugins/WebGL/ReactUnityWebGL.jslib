mergeInto(LibraryManager.library, {
  OrbGuiLoaded: function () {
    ReactUnityWebGL.OrbGuiLoaded();
  },
  CoinsUpdated: function (coins) {
    ReactUnityWebGL.CoinsUpdated(coins);
  }
});