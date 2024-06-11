/// <reference path="../node_modules/@types/chrome/index.d.ts"/>

chrome.runtime.onInstalled.addListener(function () {
  chrome.contextMenus.create({
    title: "Hurl the Page",
    contexts: ["page"],
    id: "hurl_page",
  });

  chrome.contextMenus.create({
    title: "Hurl the Link",
    contexts: ["link"],
    id: "hurl_link",
  });
});

chrome.contextMenus.onClicked.addListener(async (info, _) => {
  chrome.runtime.sendNativeMessage("com.3721tools.hurl", {
    url: info.linkUrl || info.pageUrl,
  });
});
