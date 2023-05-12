function AppViewModel() {
  var self = this;

  self.apiUrl = "https://localhost:5001"; // This should be dynamically written from an env var or similar
  self.isFetching = ko.observable(false);
  self.keywords = ko.observable("efile integration");
  self.url = ko.observable("www.infotrack.com");
  self.selectedSearchEngine = ko.observable("Google");
  self.searchEngines = ko.observableArray(["Google"]);
  self.searchResults = ko.observableArray([]);
  // inspired by https://stackoverflow.com/questions/27998963/knockout-js-how-to-sort-table
  // needed a quick way to sort
  self.headers = ["searchDate", "positions", "keywords", "url", "searchEngine"];
  self.sortHeader = ko.observable();
  self.sortDirection = ko.observable(1);
  self.toggleSort = function (header) {
    if (header === self.sortHeader()) {
      self.sortDirection(self.sortDirection() * -1);
    } else {
      self.sortHeader(header);
      self.sortDirection(1);
    }
  };

  // use a computed to subscribe to both self.sortHeader() and self.sortDirection()
  self.sortResults = ko.computed(function () {
    var sortHeader = self.sortHeader(),
      dir = self.sortDirection();
    if (!sortHeader) return;
    self.searchResults.sort(function (a, b) {
      let va;
      let vb;
      if (sortHeader === "positions") {
        (va = Number(ko.unwrap(a[sortHeader]).split(",")[0])),
          (vb = Number(ko.unwrap(b[sortHeader]).split(",")[0]));
      } else {
        (va = ko.unwrap(a[sortHeader])), (vb = ko.unwrap(b[sortHeader]));
      }
      return va < vb ? -dir : va > vb ? dir : 0;
    });
    self.searchResults.notifySubscribers();
  });

  self.search = function () {
    var data = {
      keywords: self.keywords(),
      url: self.url(),
      searchEngine: self.selectedSearchEngine(),
    };

    self.isFetching(true);

    fetch(`${self.apiUrl}/Search`, {
      mode: "cors",
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
      body: JSON.stringify(data),
    })
      .then((response) => {
        self.isFetching(false);
        return response.json();
      })
      .then((result) => {
        self.searchResults.unshift(result);
        self.sortResults();
      })
      .catch((error) => {
        self.isFetching(false);
        console.error("Error fetching search results:", error);
      });
  };
}

window.appViewModel = new AppViewModel();
ko.applyBindings(window.appViewModel);

(function () {
  var self = window.appViewModel;
  self.sortHeader(self.headers[0]);

  const today = new Date();
  const thirtyDaysAgo = new Date();
  thirtyDaysAgo.setDate(today.getDate() - 30);

  const startDate = thirtyDaysAgo.toISOString();
  const endDate = today.toISOString();

  self.isFetching(true);

  fetch(
    `${self.apiUrl}/Search/GetRange?startDate=${startDate}&endDate=${endDate}`
  )
    .then((response) => {
      self.isFetching(false);
      return response.json();
    })
    .then((result) => {
      self.searchResults(result);
    })
    .catch((error) => {
      self.isFetching(false);
      console.error("Error:", error);
    });
})();
