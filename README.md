# WGE Zenject Demo

A demo project showing how to integrate [WorldGraphEditor](https://assetstore.unity.com/packages/slug/306228) with the Zenject DI container: a custom `ITransitionManager` implementation, responsibility split into small services around scene transitions.

Architecture overview: [documentation](https://intareu.com/resources/zenject)

## Requirements

- **WorldGraphEditor** 1.3+
- **Zenject**
- **Unity 6.0+**

> WorldGraphEditor is not included in this repository and requires a separate license.

## Getting Started

1. Clone or download this repository and open the folder in Unity Hub.
2. Install **WorldGraphEditor** from the Asset Store into the project.
3. Install **Zenject** (via Package Manager or Asset Store).
4. Open any scene from the graph listed in Build Settings and press Play.
