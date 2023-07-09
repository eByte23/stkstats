import Image from 'next/image'
import { Inter } from 'next/font/google'
import { GetStaticProps, InferGetStaticPropsType } from 'next'

import fs from 'fs'
import path from 'path'
import { Game, RecentGames } from '@/types'

const inter = Inter({ subsets: ['latin'] })

type HomeProps = {
  recentGames: RecentGames
}

const Home = (props: HomeProps) => {
  console.debug("--Props",props);
  return (
    <main
      className={`flex min-h-screen flex-col items-center justify-between p-24 ${inter.className}`}
    >
      <RecentGamesList games={props.recentGames} />  
    </main>
  )
}

type RecentGamesProps = {
  games: RecentGames
}

const getScore = (game: Game): number[] => ([
  game.HomeAway === 'Home' ? game.HomeRuns : game.AwayRuns,
  game.HomeAway !== 'Home' ? game.HomeRuns : game.AwayRuns,
])

const RecentGamesList = ({ games }: RecentGamesProps) => {


  return (
    <div>
      <h1>Recent Games</h1>

      {games.map((game) => {
        const score = getScore(game)

        return (
          <div key={game.GameId}>
            <h2>{score[0]} - {score[1]}</h2>
            <h3>{game.TeamName} vs. {game.OppositionName}</h3>
          </div>
        )
      })}
    </div>
  )
}


export default Home

export const getStaticProps: GetStaticProps<HomeProps> = async () => {

  const content = fs.readFileSync(process.cwd() + "/data/recent-games.json").toString()

  const recentGames = JSON.parse(content) as RecentGames || []

  console.log(content)
  console.log(recentGames)

  return {
    props: {
      recentGames: recentGames || [],
    },
  }
}

