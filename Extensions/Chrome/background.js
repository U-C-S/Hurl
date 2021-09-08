/// <reference path="../../node_modules/@types/chrome/index.d.ts"/>

// chrome.runtime.onInstalled.addListener(async () => {
//   let url = "hurl://";
//   let tab = await chrome.tabs.create({ url });
// });

chrome.runtime.onInstalled.addListener(function () {
  chrome.contextMenus.create({
    title: "Hurl the Page",
    contexts: ["page", "link"],
    id: "myContextMenuId",
  });
});

function OpenHurl(link) {
  console.log("lol");
  let anchor = document.createElement("a");
  anchor.href = link;
  anchor.click();
}

chrome.contextMenus.onClicked.addListener(function (info, tab) {
  chrome.tabs.create({
    url: "hurl://" + info.linkUrl,
  });

  // chrome.scripting.executeScript({
  //   target: null,
  //   func: OpenHurl,
  //   args: [info.pageUrl],
  // });
});

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
