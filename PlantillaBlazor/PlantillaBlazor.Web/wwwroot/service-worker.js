const staticCarneAP = "PlantillaBlazor";
const assets = [];

self.addEventListener("install", installEvent => {
    installEvent.waitUntil(
        caches.open(staticCarneAP).then(cache => {
            cache.addAll(assets);
        })
    );
});

self.addEventListener("fetch", fetchEvent => {
    fetchEvent.respondWith(
        caches.match(fetchEvent.request).then(res => {
            return res || fetch(fetchEvent.request);
        })
    );
});
