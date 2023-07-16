import Image from 'next/image'
import { Inter } from 'next/font/google'
import { GetStaticProps, InferGetStaticPropsType } from 'next'

import fs from 'fs'
import path from 'path'
import { Game, RecentGames, Teams } from '@/types'
import Link from 'next/link'

const inter = Inter({ subsets: ['latin'] })

type HomeProps = {
  recentGames: RecentGames
  teams: Teams
}

const Home = (props: HomeProps) => {
  return (
    <div
      className={`flex min-h-screen flex-row pt-4 gap-4 ${inter.className}`}
    >

      <div className="w-1/2">
        <RecentGamesList games={props.recentGames} />


      </div>
      <div className="w-1/2">
        <TeamsList teams={props.teams} />
      </div>


    </div>
  )
}

type RecentGamesProps = {
  games: RecentGames
}

const getScore = (game: Game): number[] => ([
  game.HomeAway === 'Home' ? game.HomeRuns : game.AwayRuns,
  game.HomeAway !== 'Home' ? game.HomeRuns : game.AwayRuns,
])

const ResultHeading = ({ game }: { game: Game }) => {

  const score = getScore(game)

  switch (game.Result) {
    case 'W':
      return (
        <h2>
          <span className="text-green-500">{game.Result}</span> {score[0]} - {score[1]}
        </h2>
      )
    case 'L':
      return (
        <h2>
          <span className="text-red-500">{game.Result}</span> {score[0]} - {score[1]}
        </h2>
      )
    case 'D':
    default:
      return (
        <h2>{game.Result} {score[0]} - {score[1]}</h2>
      )
  }
}

const RecentGamesList = ({ games }: RecentGamesProps) => {


  return (
    <div className="">
      <h1 className="dark:text-white text-xl font-medium mb-2 px-2">Recent Games</h1>

      {games.map((game) => {
        const score = getScore(game)

        return (
          <div key={game.GameId} className="py-2 px-2 hover:bg-slate-900/20 dark:hover:bg-white/20">
            <Link prefetch={true} href={`games/${game.GameShortId}`} >
              <ResultHeading game={game} />
              <h3>{game.TeamName} vs. {game.OppositionName}</h3>
            </Link>
          </div>
        )
      })}
    </div>
  )
}

const TeamsList = ({ teams }: { teams: Teams }) => (
  <div>
    <h2 className="dark:text-white text-xl font-medium mb-2 px-2">Teams</h2>
    {Object.keys(teams).map((teamId) => {
      const team = teams[teamId]
      return (
        <div key={teamId} className="py-2 px-2 hover:bg-slate-900/20 dark:hover:bg-white/20">
          <Link prefetch={true} href={`teams/${team.ShortId}`} >
            <h3>{team.TeamName}</h3>
            <div className="flex flex-row justify-between">
              <span className="text-sm">{team.SeasonName}</span>
              <span className="text-sm">{team.Record}</span>
            </div>
          </Link>
        </div>
      )
    })}
  </div>
)



export default Home

export const getStaticProps: GetStaticProps<HomeProps> = async () => {

  const recentGamesContent = fs.readFileSync(process.cwd() + "/data/recent-games.json").toString()
  const teamsContent = fs.readFileSync(process.cwd() + "/data/teams.json").toString()

  const recentGames = JSON.parse(recentGamesContent) as RecentGames
  const teams = (JSON.parse(teamsContent) as Teams || {})

  return {
    props: {
      recentGames: recentGames || [],
      teams: teams || {},
    },
  }
}

