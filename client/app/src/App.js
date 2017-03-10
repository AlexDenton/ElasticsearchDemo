import React, { Component } from 'react';
import './App.css';

class App extends Component {
  render() {
    return (
      <div className="wrapper">
        <div className="left">
          <SearchInput />
        </div>
        <div className="right">
          <div className="right-inner">
            <SearchResults />
          </div>
        </div>
		  </div>
    );
  }
}

class SearchInput extends Component {
  render () {
    return (
      <input className="search-input" placeholder="Search" type="text" />
    );
  }
}

class SearchResults extends Component {
  
  render () {
    const searchResults = [
      {
        "name": "Einstein: His Life and Universe"
      },
      {
        "name": "Head First Design Patterns"
      }
    ];

    const searchResultItems = searchResults.map((searchResult) => 
      <div className="search-result">
        {searchResult.name}
			</div>
    )
    return (
      <div>{searchResultItems}</div>
    );
  }
}

export default App;
