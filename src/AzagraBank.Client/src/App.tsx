import { useEffect, useState } from 'react'
import './App.css'

const URL = import.meta.env.VITE_AZAGRABANK_API_URL;

function App() {
  const [forecasts, setForecasts] = useState<any>([])

  const requestWeather = async () => {
    const weather = await fetch(`${URL}/api/weatherforecast`);
    const weatherJson = await weather.json();
    setForecasts(weatherJson);
  }

  useEffect(() => {
    requestWeather();
  }, [])

  return (
    <div className="App">
      <table>
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {(
            forecasts ?? [
              {
                date: "N/A",
                temperatureC: "",
                temperatureF: "",
                summary: "No forecasts"
              }
            ]
          ).map((w: any) => {
            return (
              <tr key={w.date}>
                <td>{w.date}</td>
                <td>{w.temperatureC}</td>
                <td>{w.temperatureF}</td>
                <td>{w.summary}</td>
              </tr>
            )
          })}
        </tbody>
      </table>
    </div>
  )
}

export default App
