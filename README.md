<h1 align="center">Welcome to com.gameframe.serviceprovider 👋</h1>
<p>
  <img alt="Version" src="https://img.shields.io/badge/version-1.0.0-blue.svg?cacheSeconds=2592000" />
  <a href="https://twitter.com/coryleach">
    <img alt="Twitter: coryleach" src="https://img.shields.io/twitter/follow/coryleach.svg?style=social" target="_blank" />
  </a>
</p>

> A simplified ServiceProvider implementation for use in Unity3D.</br>
> Provides singleton and transient services.</br>
> Does not do dependency graphs, property or constructor injection.</br>
> Kind of just a glorified singleton manager.</br>

## Install 

#### Using UnityPackageManager (for Unity 2018.3 or later)

Find the manifest.json file in the Packages folder of your project and edit it to look like this:
```js
{
  "dependencies": {
    "com.gameframe.serviceprovider": "https://github.com/coryleach/UnityServiceProvider.git#1.0.0",
    ...
  },
}
```

## Quick Start

### Add a Singleton Service and Get it
```c#
//In your game bootstrapper script
var myService = new MyService(); //MyService implements custom interface IMyService
ServiceCollection.Current.AddSingleton(myService);

...

//In some other script someplace
var myService = ServiceProvider.Current.Get<IMyService>();
myService.DoStuff();
```

## Author

👤 **Cory Leach**

* Twitter: [@coryleach](https://twitter.com/coryleach)
* Github: [@coryleach](https://github.com/coryleach)

## Show your support

Give a ⭐️ if this project helped you!

***
_This README was generated with ❤️ by [readme-md-generator](https://github.com/kefranabg/readme-md-generator)_
