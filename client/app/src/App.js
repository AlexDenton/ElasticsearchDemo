import React, { Component } from 'react';
import './App.css';

class App extends Component {

  constructor(props) {
    super(props);
    this.state = {
      searchResults: []
    }
    this.handleSearchResultsChange = this.handleSearchResultsChange.bind(this);
  }

  handleSearchResultsChange(searchResults) {
    this.setState({
      "searchResults": searchResults
    });
  }

  render() {
    const searchResults = this.state.searchResults;

    return (
      <div className="wrapper">
        <div className="left">
          <SearchInput onSearchResultsChange={this.handleSearchResultsChange}/>
        </div>
        <div className="right">
          <div className="right-inner">
            <SearchResults searchResults={searchResults}/>
          </div>
        </div>
		  </div>
    );
  }
}

class SearchInput extends Component {
  constructor(props) {
    super(props);
    this.handleSearchInputChange = this.handleSearchInputChange.bind(this);
    this.handleSearchSettingsChange = this.handleSearchSettingsChange.bind(this);

    this.state = {
      searchRequest: {
        query: "",
        searchSettings: {
          standardAnalyzer: true,
          snowballAnalyzer: false,
          edgeNGramAnalyzer: false,
          phraseMatching: false,
          fieldBoosting: false
        }
      }
    }
  }

  getSearchResults() {
    var requestHeaders = new Headers();
    requestHeaders.append("Content-Type", "application/json");
    
    var requestBody = JSON.stringify(this.state.searchRequest);

    var requestInit = { 
      method: 'POST',
      headers: requestHeaders,
      mode: 'cors',
      cache: 'default',
      body: requestBody
    };

    var request = new Request("http://localhost:12201/search", requestInit);

    fetch(request)
      .then(function(response) {
        return response.json();
      })
      .then(function(jsonResponse) {
        this.props.onSearchResultsChange(jsonResponse.results);
      }.bind(this));
  }

  handleSearchInputChange(event) {
    var newSearchRequest = this.state.searchRequest;
    newSearchRequest.query = event.target.value;

    this.setState({
      searchRequest: newSearchRequest
    });

    this.getSearchResults();
  }

  handleSearchSettingsChange(event) {
    var newSearchRequest = this.state.searchRequest;
    newSearchRequest.searchSettings[event.target.name] = event.target.checked;
    
    this.setState({
      searchRequest: newSearchRequest
    })

    this.getSearchResults();
  }

  render () {
    return (
      <div className="search-container">
        <input className="search-input" placeholder="Search" type="text" onChange={this.handleSearchInputChange} />
        <SearchSettings searchSettings={this.state.searchRequest.searchSettings} onSearchSettingsChange={this.handleSearchSettingsChange}/>
      </div>
    );
  }
}

class SearchSettings extends Component {

  constructor(props) {
    super(props);
    this.handleInputChange = this.handleInputChange.bind(this);
  }

  handleInputChange(event) {
    this.props.onSearchSettingsChange(event);
  }

  render() {
    return (
      <div className="search-settings-container">
        <label><input type="checkbox" name="standardAnalyzer" checked={this.props.searchSettings.standardAnalyzer} onChange={this.handleInputChange} /> Standard</label>
        <label><input type="checkbox" name="snowballAnalyzer" checked={this.props.searchSettings.snowballAnalyzer} onChange={this.handleInputChange} /> Snowball</label>
        <label><input type="checkbox" name="edgeNGramAnalyzer" checked={this.props.searchSettings.edgeNGramAnalyzer} onChange={this.handleInputChange} /> Edge NGram</label>
        <label><input type="checkbox" name="phraseMatching" checked={this.props.searchSettings.phraseMatching} onChange={this.handleInputChange} /> Phrase Matching</label>
        <label><input type="checkbox" name="fieldBoosting" checked={this.props.searchSettings.fieldBoosting} onChange={this.handleInputChange} /> Field Boosting</label>
      </div>
    )
  }
}

class SearchResults extends Component {
  
  render () {
    const searchResults = this.props.searchResults;

    const searchResultItems = searchResults.map((searchResult) => 
      <div key={searchResult.id} className="search-result">
          <div className="search-result-name">{searchResult.name}</div>
          <div className="search-result-plot-summary">{searchResult.plotSummary}</div>
			</div>
    )
    return (
      <div>{searchResultItems}</div>
    );
  }
}

export default App;
