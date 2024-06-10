/// <reference path="../node_modules/@types/chrome/index.d.ts"/>

// chrome.runtime.onInstalled.addListener(async () => {
//   let url = "hurl://";
//   let tab = await chrome.tabs.create({ url });
// });

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

function OpenHurl(link) {
  let anchor = document.createElement("a");
  anchor.href = "hurl://" + link;
  anchor.click();
}

async function getCurrentTab() {
  let queryOptions = { active: true, currentWindow: true };
  let [tab] = await chrome.tabs.query(queryOptions);
  return tab;
}

chrome.contextMenus.onClicked.addListener(async function (info, tab) {
  let LINK = info.linkUrl || info.pageUrl;

  let currentTab = await getCurrentTab();

  console.log("Sending " + LINK);
  chrome.runtime.sendNativeMessage(
    "com.3721tools.hurl",
    { url: LINK },
    function (response) {
      console.log("Received " + response);
    }
  );
});

// chrome.tabs.create({
//   url: "hurl://" + info.linkUrl,
// });

// chrome.contextMenus.create({
//   contexts: ["page", "link", "page_action"],
//   enabled: true,
//   title: "Hurl the page",
//   onclick: (info, tab) => {
//     console.log(info);
//     console.log(tab);
//     window.open("hurl://");
//   },
//   id: "hurl-page-action",
// });
